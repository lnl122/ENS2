// Copyright © 2018 Antony S. Ovsyannikov aka lnl122 aka полвторого@en
// License: http://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ENS2
{
    /// <summary>
    /// методы работы с игровым движком
    /// </summary>
    interface IEngine
    {
        bool Logon(string user, string pass, string domain="");
        void Logoff();
        List<GameInfo> GetGames();
        //List<GameInfo> GetGameList();
        //void SetGame(GameInfo selected_game);
        //string GetPage(string url);// походу он станет приватным
        //string GetLevelPage(int level = -1);
        //string SendAnswer(string answer, int level = -1);
    }

    public class GameInfo
    {
        public string Type { get; private set; }
        public bool IsCommand { get; private set; }
        public bool IsStorm { get; private set; }
        public string Begin_date { get; private set; }
        public string Name { get; private set; }
        public string Link { get; private set; }
        public bool IsPlayable { get; private set; }

        /// <summary>
        /// по данным из ссылки заполняет сведения в себе
        /// </summary>
        /// <param name="url">ссылка на информацию об игре</param>
        public GameInfo(string url)
        {
            // пример входящей строки - http://pnz.en.cx/GameDetails.aspx?gid=59465
            string page = (Engine.Instance).GetPageClean(url);

        }
    }

    public sealed class Engine : IEngine
    {
        // синглтон
        private static readonly Object s_lock = new Object();
        private static Engine instance = null;
        public static Engine Instance
        {
            get
            {
                if (instance != null) return instance;
                System.Threading.Monitor.Enter(s_lock);
                Engine temp = new Engine();
                System.Threading.Interlocked.Exchange(ref instance, temp);
                System.Threading.Monitor.Exit(s_lock);
                return instance;
            }
        }

        // константы
        private static int[] gameTypes = new int[] { 0, 7, 1, 9 }; // Схватка, Точки, МШ, Викторина

        // лог
        private static Log Log = new Log("Engine");

        // куки
        private static string cHead;
        private static CookieContainer cCont;
        // блокировка операций с движком
        private static object LockAction = new object();

        // пользователь
        public static string UserName { get; private set; }
        public static string UserPass { get; private set; }
        public static string UserId { get; private set; }
        public static string UserDomain { get; private set; }
        public static string UserTeam { get; private set; }
        public static string UserTeamId { get; private set; }

        // игра
        public static string GameDomain { get; private set; }
        public static string GameId { get; private set; }
        public static string GameUrl { get; private set; }

        // флаги
        public static bool IsLoggedUser { get; private set; }
        public static bool IsLoggedInGame { get; private set; }
        public static bool IsReady { get; private set; }

        /// <summary>
        /// конструктор
        /// </summary>
        private Engine()
        {
            UserName = "";
            UserPass = "";
            UserId = "";
            UserDomain = "";
            UserTeam = "";
            UserTeamId = "";
            GameDomain = "";
            GameId = "";
            GameUrl = "";
            IsLoggedUser = false;
            IsLoggedInGame = false;
            IsReady = false;
        // инициализации движка
    }

        public bool Logon(string user, string pass, string domain="")
        {
            // 2do проверка хоста на валидность
            string domainToLogon = domain;
            if(domain == "")
            {
                string domainsString = Properties.Settings.Default.Engine_Domains;
                string[] domainsList = domainsString.Split(';');
                domainToLogon = domainsList[0]; // 2do
            }
            // выполняем логон
            string page = DoLogon(user, pass, domainToLogon);
            if (EngineParsePage.IsLogonSussefully(page))
            {
                UserName = user;
                UserPass = pass;
                string page2 = GetPage("http://" + domainToLogon + "/UserDetails.aspx");
                UserId = EngineParsePage.GetUserId(page2);
                UserDomain = EngineParsePage.GetUserDomain(page2);
                UserTeam = EngineParsePage.GetUserTeam(page2);
                UserTeamId = EngineParsePage.GetUserTeamId(page2);
                Properties.Settings.Default.User_Name = UserName;
                Properties.Settings.Default.User_Password = UserPass;
                Properties.Settings.Default.User_Id = UserId;
                Properties.Settings.Default.User_Domain = UserDomain; // 2do
                Properties.Settings.Default.User_Team = UserTeam;
                Properties.Settings.Default.User_TeamId = UserTeamId;
                Properties.Settings.Default.Save();
                IsLoggedUser = true;
                return true;
            }
            return false;
        }

        public void Logoff()
        {
            IsLoggedUser = false;
            IsLoggedInGame = false;
            IsReady = false;
        }

        /// <summary>
        /// получает список игр из сведений игрока
        /// </summary>
        /// <returns>список GameInfo - информаций об играх, не оконченных/не завершенных</returns>
        public List<GameInfo> GetGames()
        {
            List<GameInfo> result = new List<GameInfo>();
            if (IsLoggedUser)
            {
                // типы игр (Схватка, Точки, МШ, Викторина) - указаны в константах к классу
                foreach (int num in gameTypes)
                {
                    // http://tselina.en.cx/UserDetails.aspx?zone=0&tab=1&uid=1509025
                    string url = "http://" + UserDomain + "/UserDetails.aspx?zone=" + num.ToString() + "&tab=1&uid=" + UserId;
                    string page = GetPageClean(url);
                    List<string> links = EngineParsePage.GetGamesLinks(page);
                    foreach (string link in links)
                    {
                        result.Add(new GameInfo(link));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// выполняет логон в домене игры, если домен не установлен - то в game.en.cx
        /// заполняет внутренние поля, сохраняет куки, устанавливает флаг логона
        /// </summary>
        /// <returns>страница с ответом</returns>
        private string DoLogon(string user, string pass, string domain)
        {
            string pageSource = "";
            lock (LockAction)
            {
                string formParams = string.Format("Login={0}&Password={1}", user, pass);
                string cookieHeader = "";
                CookieContainer cookies = new CookieContainer();
                cCont = cookies;
                string url1 = "http://" + domain + "/Login.aspx";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url1);
                req.CookieContainer = cookies;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "POST";
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(formParams);
                req.ContentLength = bytes.Length;
                using (Stream os = req.GetRequestStream())
                {
                    os.Write(bytes, 0, bytes.Length);
                }
                try
                {
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    cookieHeader = resp.Headers["Set-cookie"];
                    cHead = cookieHeader;
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        pageSource = sr.ReadToEnd();
                    }
                }
                catch
                {
                    Log.Write("ERROR: не удалось получить ответ на авторизацию " + url1 + " / " + user + " / " + pass);
                }
            }
            return pageSource;
        }

        /// <summary>
        /// получает страницу, учитывая сохраненные куки
        /// </summary>
        /// <param name="url">урл</param>
        /// <returns>текст страницы</returns>
        public string GetPage(string url)
        {
            string page = GetPageClean(url);
            if (EngineParsePage.IsNeedLogon(page))
            {
                string logon_res = DoLogon(UserName, UserPass, UserDomain);
                if (EngineParsePage.IsLogonSussefully(logon_res))
                {
                    page = GetPageClean(url);
                }
                else
                {
                    page = "";
                }
            }
            return page;
        }

        /// <summary>
        /// получает страницу, учитывая сохраненные куки
        /// </summary>
        /// <param name="url">урл</param>
        /// <returns>текст страницы</returns>
        public string GetPageClean(string url)
        {
            string page = "";
            lock (LockAction)
            {
                HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(url);
                try
                {
                    getRequest.CookieContainer = cCont;
                    WebResponse getResponse = getRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(getResponse.GetResponseStream()))
                    {
                        page = sr.ReadToEnd();
                    }
                }
                catch
                {
                    Log.Write("ERROR: Не удалось прочитать страницу " + url);
                    page = "";
                }
            }
            return page;
        }

    }
}

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
        public string type { get; private set; }
        public bool isCommand { get; private set; }
        public bool isStorm { get; private set; }
        public string begin_date { get; private set; }
        public string name { get; private set; }
        public string link { get; private set; }
        public bool isPlayable { get; private set; }

        public GameInfo(string url)
        {
            // получить из урла все параметры
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
        public static bool isLoggedUser { get; private set; }
        public static bool isLoggedInGame { get; private set; }
        public static bool isReady { get; private set; }

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
            isLoggedUser = false;
            isLoggedInGame = false;
            isReady = false;
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
            if (isLogonSussefully(page))
            {
                UserName = user;
                UserPass = pass;
                string page2 = GetPage("http://" + domainToLogon + "/UserDetails.aspx");
                UserId = GetUserId(page2);
                UserDomain = GetUserDomain(page2);
                UserTeam = GetUserTeam(page2);
                UserTeamId = GetUserTeamId(page2);
                Properties.Settings.Default.User_Name = UserName;
                Properties.Settings.Default.User_Password = UserPass;
                Properties.Settings.Default.User_Id = UserId;
                Properties.Settings.Default.User_Domain = UserDomain; // 2do
                Properties.Settings.Default.User_Team = UserTeam;
                Properties.Settings.Default.User_TeamId = UserTeamId;
                Properties.Settings.Default.Save();
                isLoggedUser = true;
                return true;
            }
            return false;
        }

        public void Logoff()
        {
            isLoggedUser = false;
            isLoggedInGame = false;
            isReady = false;
        }

        public List<GameInfo> GetGames()
        {
            List<GameInfo> result = new List<GameInfo>();
            if (isLoggedUser)
            {
                foreach (int num in gameTypes)
                {
                    result.AddRange(GetGamesByType(num));
                }
            }
            return result;
        }

        public List<GameInfo> GetGamesByType(int v)
        {
            List<GameInfo> result = new List<GameInfo>();
            // http://tselina.en.cx/UserDetails.aspx?zone=0&tab=1&uid=1509025
            string url = "http://" + UserDomain + "/UserDetails.aspx?zone=" + v.ToString() + "&tab=1&uid=" + UserId;
            string page = GetPageClean(url);

            List<string> links = GetGamesLinks(page);
            foreach (string link in links)
            {
                result.Add(new GameInfo(link));
            }
            return result;
        }

        public List<string> GetGamesLinks(string page)
        {
            List<string> result = new List<string>();
            int idx = page.IndexOf("/UserDetails.aspx?tab=0&zone=1&uid=");
            if(idx > 0)
            {
                string p = page.Substring(idx);
                int idx1 = p.IndexOf("/GameDetails.aspx?gid=") - 100; // сто символов назад - для наверняка
                int idx2 = p.LastIndexOf("/GameDetails.aspx?gid=") + 100; // сто символов вперед - для наверняка
                if((idx1 > 0) && (idx2 > 0) && (idx1 < idx2))
                {
                    p = p.Substring(idx1, idx2-idx1);
                    string[] arr = System.Text.RegularExpressions.Regex.Split(p, "href=\"");
                    foreach(string str in arr)
                    {
                        string str1 = str.Substring(0, str.IndexOf("\""));
                        if (str1.Contains("/GameDetails.aspx?gid="))
                        {
                            result.Add(str1);
                        }
                    }

                }

            }
            return result;
        }

        public string GetUserId(string page)
        {
            string p = page.Substring(page.IndexOf("&uid=")+5);
            p = p.Substring(0, p.IndexOf("\""));
            return p;
        }

        public string GetUserDomain(string page)
        {
            string p = page.Substring(page.IndexOf("Прописка:") + 9);
            p = p.Substring(p.IndexOf("href=\"") + 6);
            p = p.Substring(0, p.IndexOf("/\""));
            p = p.Replace("http://", "");
            return p;
        }

        public string GetUserTeam(string page)
        {
            int idx = page.IndexOf("/Teams/TeamDetails.aspx?tid=");
            if (idx == -1)
            {
                return "";  
            }
            string p = page.Substring(idx + 28);
            p = p.Substring(p.IndexOf(">")+1);
            p = p.Substring(0, p.IndexOf("<"));
            return p;
        }

        public string GetUserTeamId(string page)
        {
            int idx = page.IndexOf("/Teams/TeamDetails.aspx?tid=");
            if(idx == -1)
            {
                return "";
            }
            string p = page.Substring(idx + 28);
            p = p.Substring(0, p.IndexOf("\""));
            return p;
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
        /// проверяет страницу на предмет поиска строки с logout
        /// </summary>
        /// <param name="page">код страницы</param>
        /// <returns>true - если мы залогонены, false - если нужен логон</returns>
        public bool isLogonSussefully(string page)
        {
            if (page.IndexOf("action=logout") != -1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// проверяет страницу на предмет поиска строки с Login.aspx
        /// </summary>
        /// <param name="page">код страницы</param>
        /// <returns>true - есть необходимость в логоне</returns>
        public bool isNeedLogon(string page)
        {
            if (page.IndexOf("action=\"/Login.aspx") != -1)
            {
                return true;
            }
            return false;
        }

        ///// <summary>
        ///// выбирает конкретную игру
        ///// </summary>
        ///// <param name="selected_game">выбранная игра</param>
        //public void SetGame(GameInfo selected_game)
        //{
        //    // скопировать в объект данные игры
        //    // создать строки урл для получения уровней
        //    // 
        //    throw new NotImplementedException("устанавливаем параметры игры, реальных гейминфо пока нет");

        //    if (isLogged) { isReady = true; }
        //}

        ///// <summary>
        ///// получает список игр, на которые подписан пользователь
        ///// </summary>
        ///// <returns>список структур с описанием игр</returns>
        //public List<GameInfo> GetGameList()
        //{
        //    if (!isLogged) { return new List<GameInfo>(); }
        //    // получить список игр пользователя
        //    // выбрать неигранные, создать список
        //    // для дебуга и себя - добавить игры с демо.ен.цх

        //    throw new NotImplementedException("не можем создать список игр");
        //}

        ///// <summary>
        ///// получает информацию об одной игре
        ///// </summary>
        ///// <param name="domain">домен игры</param>
        ///// <param name="gamenumber">номер игры</param>
        ///// <returns>структура данных об игре</returns>
        //private GameInfo GetGameInfo(string domain, string gamenumber)
        //{
        //    // прочитать описание игры по ссылке, вычленить параметры игры, сложить в структуру

        //    throw new NotImplementedException("получили страницу с описанием игры, но создать объект гейминфо не можем.");
        //}

        ///// <summary>
        ///// пробует отправить ответ в игровой движек,
        ///// при запросе авторизации - выполняет её
        ///// </summary>
        ///// <param name="answer">ответ</param>
        ///// <param name="level">номер уровня для штурма, или пусто для линейки</param>
        ///// <returns>страница, полученная в ответ</returns>
        //public string SendAnswer(string answer, int level = -1)
        //{
        //    if (!isReady) { return ""; }

        //    // если в ответной странице isNeedLogon надо переавторизоваться и, если успешно - повторить отправку

        //    throw new NotImplementedException("как бы выполнили отправку ответа, но вернуть страницу с результатом мы не можем.");
        //}

        /// <summary>
        /// получает страницу, учитывая сохраненные куки
        /// </summary>
        /// <param name="url">урл</param>
        /// <returns>текст страницы</returns>
        public string GetPage(string url)
        {
            string page = GetPageClean(url);
            if (isNeedLogon(page))
            {
                string logon_res = DoLogon(UserName, UserPass, UserDomain);
                if (isLogonSussefully(logon_res))
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

        ///// <summary>
        ///// получает страницу с уровнем
        ///// </summary>
        ///// <param name="level">уровень, если есть необходимость в его указании для штурма, или пусто для линейки</param>
        ///// <returns>текст страницы</returns>
        //public string GetLevelPage(int level = -1)
        //{
        //    if (!isReady) { return ""; }

        //    // если на странице встретили "<form ID=\"formMain\" method=\"post\" action=\"/Login.aspx?return=%2fgameengines%2fencounter%2fplay%2f24889%2f%3flevel%3d11"
        //    // надо переавторизоваться и, если успешно - вернуть страницу

        //    throw new NotImplementedException("запросили страничку с уровнем, но вернуть её мы неможем");
        //}
    }
}

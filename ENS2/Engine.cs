// Copyright © 2018 Antony S. Ovsyannikov aka lnl122 aka полвторого@en
// License: http://opensource.org/licenses/MIT

// 2do
// - Logon - выбор случайного домена из списка через ; из настроек

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
        //List<GameInfo> GetGameList();
        //void SetGame(GameInfo selected_game);
        //string GetPage(string url);// походу он станет приватным
        //string GetLevelPage(int level = -1);
        //string SendAnswer(string answer, int level = -1);

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

        // лог
        private static Log Log = new Log("Engine");

        // куки
        private static string cHead;
        private static CookieContainer cCont;
        // блокировка операций с движком
        private static object LockAction = new object();

        // пользователь
        private static string UserName = "";
        private static string UserPass = "";
        private static string UserId = "";
        private static string UserDomain = "";
        private static string UserTeam = "";
        private static string UserTeamId = "";

        // игра
        private static string GameDomain = "";
        private static string GameId = "";
        private static string GameUrl = "";

        // флаги
        private static bool isLoggedUser = false;
        private static bool isLoggedInGame = false;
        private static bool isReady = false;

        /// <summary>
        /// конструктор
        /// </summary>
        private Engine()
        {
            // инициализации движка
        }

        public bool Logon(string user, string pass, string domain="")
        {
            string domainToLogon = domain;
            if(domain == "")
            {
                string domainsString = Properties.Settings.Default.Engine_Domains;
                string[] domainsList = domainsString.Split(';');
                domainToLogon = domainsList[0]; // 2do
            }
            // выполняем логон
            return false;
        }
    }
}

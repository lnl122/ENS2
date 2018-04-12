// Copyright © 2017 Antony S. Ovsyannikov aka lnl122
// License: http://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENS2
{

    /// <summary>
    /// методы работы с информацией о пользвоателе движка
    /// </summary>
    interface IENUser
    {
        bool Logon();
        void Logoff();
    }

    /// <summary>
    /// информация об игроке - имя, пароль
    /// </summary>
    public class ENUser : IENUser
    {
        private Log Log = new Log("ENUser");

        public string Name { get; private set; }
        public string Pass { get; private set; }
        public string Id { get; private set; }
        public string Domain { get; private set; }
        public string Team { get; private set; }
        public string TeamId { get; private set; }
        public bool IsSuccessfullyLogged { get; private set; }

        public ENUser(string user, string pass)
        {
            Name = user;
            Pass = pass;
            Id = "";
            Domain = "";
            Team = "";
            TeamId = "";
            IsSuccessfullyLogged = false;
        }

        public ENUser()
        {
            // ***122 читать всё из внешних настроек
            Name = "";
            Pass = "";
            Id = "";
            Domain = "";
            Team = "";
            TeamId = "";
            IsSuccessfullyLogged = false;
        }

        /// <summary>
        /// выполняет логон пользвоателя
        /// </summary>
        /// <returns>признак логона в движке</returns>
        public bool Logon()
        {
            // ***122
            return IsSuccessfullyLogged;
        }

        /// <summary>
        /// выполняет логофф пользвоателя
        /// </summary>
        /// <returns></returns>
        public void Logoff()
        {
            // ***122
        }
    }
}

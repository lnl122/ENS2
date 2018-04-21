// Copyright © 2018 Antony S. Ovsyannikov aka lnl122 aka полвторого@en
// License: http://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using ENS2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class EngineTests
    {
        public Engine EN = Engine.Instance;

        /*
        [TestMethod]
        public void Engine_isLogonSussefully_1()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Root.html").Replace("\n", " ");
            Assert.IsFalse(EN.isLogonSussefully(page));
        }
        [TestMethod]
        public void Engine_isLogonSussefully_2()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Logon.html").Replace("\n", " ");
            Assert.IsFalse(EN.isLogonSussefully(page));
        }
        [TestMethod]
        public void Engine_isLogonSussefully_3()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Logon_GoodPassword.html").Replace("\n", " ");
            Assert.IsTrue(EN.isLogonSussefully(page));
        }
        [TestMethod]
        public void Engine_isLogonSussefully_4()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Logon_BadPassword.html").Replace("\n", " ");
            Assert.IsFalse(EN.isLogonSussefully(page));
        }
        [TestMethod]
        public void Engine_isLogonSussefully_5()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials_woTeam.html").Replace("\n", " ");
            Assert.IsTrue(EN.isLogonSussefully(page));
        }

        [TestMethod]
        public void Engine_isNeedLogon_1()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Root.html").Replace("\n", " ");
            Assert.IsFalse(EN.isNeedLogon(page));
        }
        [TestMethod]
        public void Engine_isNeedLogon_2()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Logon.html").Replace("\n", " ");
            Assert.IsTrue(EN.isNeedLogon(page));
        }
        [TestMethod]
        public void Engine_isNeedLogon_3()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Logon_GoodPassword.html").Replace("\n", " ");
            Assert.IsFalse(EN.isNeedLogon(page));
        }
        [TestMethod]
        public void Engine_isNeedLogon_4()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Logon_BadPassword.html").Replace("\n", " ");
            Assert.IsTrue(EN.isNeedLogon(page));
        }
        [TestMethod]
        public void Engine_isNeedLogon_5()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials_woTeam.html").Replace("\n", " ");
            Assert.IsFalse(EN.isNeedLogon(page));
        }

        //[TestMethod]
        public void Engine_Logon_1()
        {
            Assert.IsTrue(EN.Logon("полвторого", "пароль", "game.en.cx"));
        }
        //[TestMethod]
        public void Engine_Logon_2()
        {
            Assert.IsFalse(EN.Logon("полвторого122", "пароль", "game.en.cx"));
        }
        [TestMethod]
        public void Engine_GetUserId_1()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials.html").Replace("\n", " ");
            Assert.AreEqual("1509025", EN.GetUserId(page));
        }
        [TestMethod]
        public void Engine_GetUserId_2()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials.html").Replace("\n", " ");
            Assert.AreNotEqual("123456123456", EN.GetUserId(page));
        }
        [TestMethod]
        public void Engine_GetUserDomain_1()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials.html").Replace("\n", " ");
            Assert.AreEqual("tselina.en.cx", EN.GetUserDomain(page));
        }
        [TestMethod]
        public void Engine_GetUserDomain_2()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials.html").Replace("\n", " ");
            Assert.AreNotEqual("game.en.cx", EN.GetUserDomain(page));
        }
        [TestMethod]
        public void Engine_GetUserTeam_1()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials.html").Replace("\n", " ");
            Assert.AreEqual("Кровостооок", EN.GetUserTeam(page));
        }
        [TestMethod]
        public void Engine_GetUserTeam_2()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials.html").Replace("\n", " ");
            Assert.AreNotEqual("123456123456", EN.GetUserTeam(page));
        }
        [TestMethod]
        public void Engine_GetUserTeam_3()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials_woTeam.html").Replace("\n", " ");
            Assert.AreEqual("", EN.GetUserTeam(page));
        }
        [TestMethod]
        public void Engine_GetUserTeamId_1()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials.html").Replace("\n", " ");
            Assert.AreEqual("142920", EN.GetUserTeamId(page));
        }
        [TestMethod]
        public void Engine_GetUserTeamId_2()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials.html").Replace("\n", " ");
            Assert.AreNotEqual("123456123456", EN.GetUserTeamId(page));
        }
        [TestMethod]
        public void Engine_GetUserTeamId_3()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials_woTeam.html").Replace("\n", " ");
            Assert.AreEqual("", EN.GetUserTeamId(page));
        }
        */
        /*
        [TestMethod]
        public void Engine_GetGamesLinks_1()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\tselina_en_cx_GameList_zone1_Open.html").Replace("\n", " ");
            List<string> res = EN.GetGamesLinks(page);
            Assert.AreEqual(3, res.Count);
        }
        [TestMethod]
        public void Engine_GetGamesLinks_1_1()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\tselina_en_cx_GameList_zone1_Open.html").Replace("\n", " ");
            List<string> res = EN.GetGamesLinks(page);
            Assert.IsTrue(res.Contains("http://odessa.en.cx/GameDetails.aspx?gid=30040"));
        }
        [TestMethod]
        public void Engine_GetGamesLinks_1_2()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\tselina_en_cx_GameList_zone1_Open.html").Replace("\n", " ");
            List<string> res = EN.GetGamesLinks(page);
            Assert.IsTrue(res.Contains("http://magadan.en.cx/GameDetails.aspx?gid=60869"));
        }
        [TestMethod]
        public void Engine_GetGamesLinks_1_3()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\tselina_en_cx_GameList_zone1_Open.html").Replace("\n", " ");
            List<string> res = EN.GetGamesLinks(page);
            Assert.IsTrue(res.Contains("http://moscow.en.cx/GameDetails.aspx?gid=58144"));
        }
        [TestMethod]
        public void Engine_GetGamesLinks_2()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\tselina_en_cx_GameList_zone3_Blank.html").Replace("\n", " ");
            List<string> res = EN.GetGamesLinks(page);
            Assert.AreEqual(0, res.Count);
        }
        [TestMethod]
        public void Engine_GetGamesLinks_3()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\tselina_en_cx_GameList_zone7_Close.html").Replace("\n", " ");
            List<string> res = EN.GetGamesLinks(page);
            Assert.AreEqual(0, res.Count);
        }
        */
    }
}

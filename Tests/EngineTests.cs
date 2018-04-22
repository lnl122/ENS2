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

        //[TestMethod]
        public void Engine_GetComplexName_1()
        {
            GameInfo g1 = new GameInfo("http://magnitka.en.cx/GameDetails.aspx?gid=59419");
            Assert.AreEqual("26.08.2017 22:00:00 Точки \"Загадочная\" / Ком. Линейка", g1.GetComplexName());
        }
        //[TestMethod]
        public void Engine_GetComplexName_2()
        {
            GameInfo g1 = new GameInfo("http://magadan.en.cx/GameDetails.aspx?gid=60869");
            Assert.AreEqual("05.03.2018 15:00:00 Мозговой штурм \"Бесконечный МШ\" / Один Линейка", g1.GetComplexName());
        }

        [TestMethod]
        public void Engine_GetGameComplex_1()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\magadan_en_cx_Game_BS_One_Line_Open.html").Replace("\n", " ");
            Assert.AreEqual("Мозговой штурм", EngineParsePage.GetGameType(page));
            Assert.IsFalse(EngineParsePage.GetIsCommand(page));
            Assert.IsFalse(EngineParsePage.GetGameIsStorm(page));
            Assert.AreEqual("05.03.2018 15:00:00", EngineParsePage.GetGameBeginDate(page));
            Assert.AreEqual("Бесконечный МШ", EngineParsePage.GetGameName(page));
            //Assert.AreEqual("http://magadan.en.cx/gameengines/encounter/play/60869", EngineParsePage.GetGameLink(page));
            Assert.IsTrue(EngineParsePage.GetGameIsPlayable(page));
        }
        [TestMethod]
        public void Engine_GetGameComplex_2()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\odessa_en_cx_Game_BS_One_Line.html").Replace("\n", " ");
            Assert.AreEqual("Мозговой штурм", EngineParsePage.GetGameType(page));
            Assert.IsFalse(EngineParsePage.GetIsCommand(page));
            Assert.IsFalse(EngineParsePage.GetGameIsStorm(page));
            Assert.AreEqual("29.07.2018 19:00:00", EngineParsePage.GetGameBeginDate(page));
            Assert.AreEqual("Олдскульный спринт", EngineParsePage.GetGameName(page));
            //Assert.AreEqual("http://odessa.en.cx/gameengines/encounter/play/30040", EngineParsePage.GetGameLink(page));
            Assert.IsTrue(EngineParsePage.GetGameIsPlayable(page));
        }
        [TestMethod]
        public void Engine_GetGameComplex_3()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\tallinn_en_cx_Game_BS_Com_Storm.html").Replace("\n", " ");
            Assert.AreEqual("Мозговой штурм", EngineParsePage.GetGameType(page));
            Assert.IsTrue(EngineParsePage.GetIsCommand(page));
            Assert.IsTrue(EngineParsePage.GetGameIsStorm(page));
            Assert.AreEqual("16.04.2018 21:00:00", EngineParsePage.GetGameBeginDate(page));
            Assert.AreEqual("Книга о вкусной и здоровой пище", EngineParsePage.GetGameName(page));
            //Assert.AreEqual("http://tallinn.en.cx/gameengines/encounter/play/60819", EngineParsePage.GetGameLink(page));
            Assert.IsFalse(EngineParsePage.GetGameIsPlayable(page));
        }

        [TestMethod]
        public void Engine_isLogonSussefullyNeed_1()
        {
            string page1 = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Root.html").Replace("\n", " ");
            Assert.IsFalse(EngineParsePage.IsLogonSussefully(page1));
            Assert.IsFalse(EngineParsePage.IsNeedLogon(page1));
            string page2 = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Logon.html").Replace("\n", " ");
            Assert.IsFalse(EngineParsePage.IsLogonSussefully(page2));
            Assert.IsTrue(EngineParsePage.IsNeedLogon(page2));
            string page3 = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Logon_GoodPassword.html").Replace("\n", " ");
            Assert.IsTrue(EngineParsePage.IsLogonSussefully(page3));
            Assert.IsFalse(EngineParsePage.IsNeedLogon(page3));
            string page4 = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Logon_BadPassword.html").Replace("\n", " ");
            Assert.IsFalse(EngineParsePage.IsLogonSussefully(page4));
            Assert.IsTrue(EngineParsePage.IsNeedLogon(page4));
            string page5 = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials_woTeam.html").Replace("\n", " ");
            Assert.IsTrue(EngineParsePage.IsLogonSussefully(page5));
            Assert.IsFalse(EngineParsePage.IsNeedLogon(page5));
        }
        [TestMethod]
        public void Engine_GetUserId_1()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials.html").Replace("\n", " ");
            Assert.AreEqual("1509025", EngineParsePage.GetUserId(page));
            Assert.AreNotEqual("123456123456", EngineParsePage.GetUserId(page));
            Assert.AreEqual("tselina.en.cx", EngineParsePage.GetUserDomain(page));
            Assert.AreNotEqual("game.en.cx", EngineParsePage.GetUserDomain(page));
            Assert.AreEqual("Кровостооок", EngineParsePage.GetUserTeam(page));
            Assert.AreNotEqual("123456123456", EngineParsePage.GetUserTeam(page));
        }
        [TestMethod]
        public void Engine_GetUserTeam_3()
        {
            string page1 = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials_woTeam.html").Replace("\n", " ");
            string page2 = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_UserDetials.html").Replace("\n", " ");
            Assert.AreEqual("", EngineParsePage.GetUserTeam(page1));
            Assert.AreEqual("142920", EngineParsePage.GetUserTeamId(page2));
            Assert.AreNotEqual("123456123456", EngineParsePage.GetUserTeamId(page2));
            Assert.AreEqual("", EngineParsePage.GetUserTeamId(page1));
        }

        [TestMethod]
        public void Engine_GetGamesLinks_1()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\tselina_en_cx_GameList_zone1_Open.html").Replace("\n", " ");
            List<string> res = EngineParsePage.GetGamesLinks(page);
            Assert.AreEqual(3, res.Count);
            Assert.IsTrue(res.Contains("http://odessa.en.cx/GameDetails.aspx?gid=30040"));
            Assert.IsTrue(res.Contains("http://magadan.en.cx/GameDetails.aspx?gid=60869"));
            Assert.IsTrue(res.Contains("http://moscow.en.cx/GameDetails.aspx?gid=58144"));
        }
        [TestMethod]
        public void Engine_GetGamesLinks_2()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\tselina_en_cx_GameList_zone3_Blank.html").Replace("\n", " ");
            List<string> res = EngineParsePage.GetGamesLinks(page);
            Assert.AreEqual(0, res.Count);
        }
        [TestMethod]
        public void Engine_GetGamesLinks_3()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\tselina_en_cx_GameList_zone7_Close.html").Replace("\n", " ");
            List<string> res = EngineParsePage.GetGamesLinks(page);
            Assert.AreEqual(0, res.Count);
        }
        
    }
}

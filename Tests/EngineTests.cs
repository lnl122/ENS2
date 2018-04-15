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
            Assert.IsTrue(EN.Logon("полвторого", "ovs122", "game.en.cx"));
        }
        //[TestMethod]
        public void Engine_Logon_2()
        {
            Assert.IsFalse(EN.Logon("полвторого122", "ovs122", "game.en.cx"));
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

    }
}

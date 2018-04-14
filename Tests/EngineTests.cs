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
        public void Engine_isNeedSussefully_1()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Root.html").Replace("\n", " ");
            Assert.IsFalse(EN.isNeedLogon(page));
        }
        [TestMethod]
        public void Engine_isNeedSussefully_2()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Logon.html").Replace("\n", " ");
            Assert.IsTrue(EN.isNeedLogon(page));
        }
        [TestMethod]
        public void Engine_isNeedSussefully_3()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Logon_GoodPassword.html").Replace("\n", " ");
            Assert.IsFalse(EN.isNeedLogon(page));
        }
        [TestMethod]
        public void Engine_isNeedSussefully_4()
        {
            string page = System.IO.File.ReadAllText(@"..\..\TestEnginePages\game_en_cx_Logon_BadPassword.html").Replace("\n", " ");
            Assert.IsTrue(EN.isNeedLogon(page));
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
        //[TestMethod]
        public void Engine_LogonInfo_1()
        {
            bool res = EN.Logon("полвторого", "ovs122", "game.en.cx");
            Assert.IsTrue(res);
            if (res)
            {
                //Assert.AreEqual("1509025", EN.)
            }
        }


    }
}

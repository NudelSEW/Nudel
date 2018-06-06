using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nudel.Backend;
namespace NudelBackendTest
{
    [TestClass]
    public class UnitTest1
    {
        
        [TestMethod]
        public void TestMongoConnection()
        {
            
            
        }
        [TestMethod]
        public void TestRegister()
        {
            string passw1 = "Passwort";
            string passw2 = "passwort";
            string name1 = "Jeff";
            string name2 = "jeff";
            NudelService ns = new NudelService();
            string ns1 = ns.Register(name1, passw1, "", "", "");
            string ns2 = ns.Register(name2, passw2, "", "", "");

            Assert.Equals(ns1,ns2);
        }
    }
}

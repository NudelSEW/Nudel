using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nudel.Backend;

namespace NudelBackendTest
{
    [TestClass]
    public class Tests
    {
        private NudelService nudel;

        public Tests()
        {
            nudel = new NudelService();
        }

        [TestMethod]
        public void TestMongoConnection()
        {


        }

        [TestMethod]
        public void TestRegister()
        {
            nudel.Register("testuser", "test@test.at", "test1234", "testname", "testnname");
            nudel.Register("testuser2", "test2@test.at", "test1234", "testname", "testnname");
        }

        [TestMethod]
        public void TestLogin()
        {
            nudel.Login("testuser", "test123");
            nudel.Login("test2@test.at", "test1234");
        }


        [TestMethod]
        public void TestFindUser()
        {
            nudel.FindUser(1);

        }
      
    }
}

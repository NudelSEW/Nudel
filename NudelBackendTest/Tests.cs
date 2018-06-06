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
            nudel.Register("testuser", "test@test.at", "test123", "testname", "testnname");
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Nudel.Backend;
using Nudel.BusinessObjects;
using System;
using System.Collections.Generic;

namespace NudelBackendTest
{
    [TestClass]
    public class Tests
    {
        private NudelService nudel;

        private MongoClient mongo;
        private IMongoDatabase db;
        private IMongoCollection<User> userCollection;
        private IMongoCollection<Event> eventCollection;

        public Tests()
        {
            nudel = new NudelService();

            mongo = new MongoClient("mongodb://nudel:nudel@docker:27017");
            db = mongo.GetDatabase("nudel");
            userCollection = db.GetCollection<User>("users");
            eventCollection = db.GetCollection<Event>("events");
        }

        [TestMethod]
        public void Should_Connect_Database()
        {
            
        }

        [TestMethod]
        public void Should_Register()
        {
            nudel.Register("testuser", "test@test.at", "test1234", "testname", "testnname");
            nudel.Register("testuser2", "test2@test.at", "test1234", "testname", "testnname");
        }

        [TestMethod]
        public void Should_Login()
        {
            nudel.Login("testuser", "test123");
            nudel.Login("test2@test.at", "test1234");
        }
        
        [TestMethod]
        public void Should_Find_User()
        {
            nudel.FindUser(1);
        }
    }
}

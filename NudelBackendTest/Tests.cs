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
        private const string sessionToken = "testToken";

        private static MongoClient mongo;
        private static IMongoDatabase db;
        private static IMongoCollection<User> userCollection;
        private static IMongoCollection<Event> eventCollection;

        static Tests()
        {
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
        public void Should_Create_Event()
        {
            string title = "Should_Create_Event() Test Event";
            Location location = new Location(46, 16);

            // Delete previous Test Objects, if still persistent
            var deleteResult = eventCollection.DeleteMany(x => x.Title == title && x.Location == location);
            long deleted = deleteResult.DeletedCount;
            Console.WriteLine($"Deleted: {deleted}");

            Event @event = new Event
            {
                Title = title,
                Description = "Just a basic test event",
                Time = DateTime.Now,
                Location = location,
                Options = new List<DateTime>(new DateTime[]
                {
                    new DateTime(2018, 6, 10),
                    new DateTime(2018, 6, 11),
                    new DateTime(2018, 6, 12),
                })
            };


            NudelService nudel = new NudelService(sessionToken);

            nudel.CreateEvent(@event);

            var results = eventCollection.Find(x => x.Title == title && x.Location == location);

            if (results.Count() == 1)
            {
                Event foundEvent = results.FirstOrDefault();
                Console.WriteLine($"Result: {foundEvent.ID}");

                // Remove Test Object after test
                eventCollection.DeleteOne(x => x.ID == foundEvent.ID);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Should_Find_User()
        {
            nudel.FindUser(1);
        }

        [TestMethod]
        public void Should_Find_Events()
        {
            NudelService nudel = new NudelService(sessionToken);

            nudel.FindEvents("TGM");
        }

        [TestMethod]
        public void Should_Find_User_String()
        {
            NudelService nudel = new NudelService(sessionToken);

            nudel.FindUser("");
        }

    }
}

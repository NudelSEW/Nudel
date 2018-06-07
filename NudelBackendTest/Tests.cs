using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
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
        private const string sessionToken = "testToken";

        private MongoClient mongo;
        private IMongoDatabase db;
        private IMongoCollection<User> userCollection;
        private IMongoCollection<Event> eventCollection;

        public Tests()
        {
            mongo = new MongoClient("mongodb://nudel:nudel@docker:27017");
            db = mongo.GetDatabase("nudel");
            userCollection = db.GetCollection<User>("users");
            eventCollection = db.GetCollection<Event>("events");

            User testUser = new User
            {
                ID = ObjectId.GenerateNewId(),
                Username = "TestUser",
                SessionToken = sessionToken
            };

            userCollection.InsertOne(testUser);
        }

        private void DeleteTestUser()
        {
            userCollection.DeleteMany(x => x.SessionToken == "testToken");
        }

        [TestMethod]
       public void Should_Register()
        {
            NudelService nudel = new NudelService();

            DeleteTestUser();
        }

        [TestMethod]
        public void Should_Login()
        {
            NudelService nudel = new NudelService();

            nudel.Login("testuser", "test123");
            nudel.Login("test2@test.at", "test1234");

            DeleteTestUser();
        }

        [TestMethod]
        public void Should_Create_Event()
        {
            string title = "Should_Create_Event() Test Event";
            Location location = new Location(46, 16);

            // Delete previous Test Objects, if still persistent
           eventCollection.DeleteMany(x => x.Title == title && x.Location == location);

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

                // Remove Test Object after test
                eventCollection.DeleteOne(x => x.ID == foundEvent.ID);
            }
            else
            {
                DeleteTestUser();
                Assert.Fail();
            }
            DeleteTestUser();
        }

        [TestMethod]
        public void Should_Find_User()
        {
            NudelService nudel = new NudelService(sessionToken);

            nudel.FindUser(ObjectId.Parse("1"));

            DeleteTestUser();
        }

        [TestMethod]
        public void Should_Find_Events()
        {
            NudelService nudel = new NudelService(sessionToken);

            nudel.FindEvents("TGM");

            DeleteTestUser();
        }

        [TestMethod]
        public void Should_Find_User_String()
        {
            NudelService nudel = new NudelService(sessionToken);

            nudel.FindUser("");

            DeleteTestUser();
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using Nudel.Backend;
using Nudel.BusinessObjects;
using Nudel.Networking;
using System;
using System.Collections.Generic;

namespace NudelBackendTest
{
    /// <summary>
    /// test cases for NudelBackend
    /// </summary>
    [TestClass]
    public class Tests
    {
        private const string sessionToken = "testToken";

        private MongoClient mongo;
        private IMongoDatabase db;
        private IMongoCollection<User> userCollection;
        private IMongoCollection<Event> eventCollection;

        /// <summary>
        /// constructor with used variables
        /// </summary>
        public Tests()
        {
            mongo = new MongoClient("mongodb://nudel:nudel@docker:27017");
            db = mongo.GetDatabase("nudel");
            userCollection = db.GetCollection<User>("users");
            eventCollection = db.GetCollection<Event>("events");

            User testUser = new User
            {
                ID = Guid.NewGuid().ToString(),
                Username = "TestUser",
                SessionToken = sessionToken
            };

            userCollection.InsertOne(testUser);
        }

        /// <summary>
        /// test if the delete function works as intented
        /// </summary>
        [TestMethod]
        private void DeleteTestUser()
        {
            userCollection.DeleteMany(x => x.SessionToken == "testToken");
        }

        /// <summary>
        /// Testing the register_function through nudelService
        /// </summary>
        [TestMethod]
       public void Should_Register()
        {
            NudelService nudel = new NudelService();
            Result result = nudel.Register(new User
            {
                Username = "chris",
                Password = "hallo123",
                Email = "chris@chris.com",
                FirstName = "Christoph",
                LastName = "Pader-Barosch"
            });

            DeleteTestUser();
        }
        /// <summary>
        /// using different parameters in the login function for testing
        /// </summary>
        [TestMethod]
        public void Should_Login()
        {
            NudelService nudel = new NudelService();

            Result result = nudel.Login(new User { Username = "chris", Password = "hallo123" });

            Assert.AreEqual(result.Type, ResultType.Success);

            DeleteTestUser();
        }
        /// <summary>
        /// testing if a new event is created when using those parameters
        /// </summary>
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
        /// <summary>
        ///  instantiating a new nudelService object to test if the user is found
        /// </summary>
        [TestMethod]
        public void Should_Find_User()
        {
            NudelService nudel = new NudelService(sessionToken);

            nudel.FindUser("TestUser");

            DeleteTestUser();
        }

        /// <summary>
        /// instantiating a new nudelService object to test if an event is found
        /// </summary>
        [TestMethod]
        public void Should_Find_Events()
        {
            NudelService nudel = new NudelService(sessionToken);

            nudel.FindEvents("TGM");

            DeleteTestUser();
        }

        /// <summary>
        /// instantiating a new nudelService object to test finding users by string values
        /// </summary>
        [TestMethod]
        public void Should_Find_User_String()
        {
            NudelService nudel = new NudelService(sessionToken);

            nudel.FindUser("");

            DeleteTestUser();
        }

    }
}

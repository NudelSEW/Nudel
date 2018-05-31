using MongoDB.Driver;
using Nudel.BusinessObjects;
using System;
using System.Collections.Generic;

namespace Nudel.Backend
{
    public class NudelService
    {
        private MongoClient mongo;
        private IMongoDatabase db;

        public NudelService()
        {
            mongo = new MongoClient(new MongoClientSettings {
                Server = new MongoServerAddress("localhost", 27017)
            });
            db = mongo.GetDatabase("nudel");
        }

        public string Register(string username, string email, string password, string firstName, string lastName)
        {
            var collection = db.GetCollection<User>("users");

            long lastID = collection.Find(x => true).SortByDescending(d => d.ID).Limit(1).FirstOrDefault().ID;

            Console.WriteLine($"Last ID: {lastID}");

            collection.InsertOne(new User
            {
                ID = lastID,
                Username = username,
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            });

            return "1234";
        }

        public string Login(string usernameOrEmail, string password) => throw new NotImplementedException();

        public void CreateEvent(string title, string description, DateTime time, Location location, List<DateTime> options) => throw new NotImplementedException();

        public Event FindEvent(long id) => throw new NotImplementedException();

        public List<Event> FindEvents(string title) => throw new NotImplementedException();

        public void SubscribeEvent(Event @event) => throw new NotImplementedException();

        public void SubscribeEvent(long id) => throw new NotImplementedException();

        public void SubscribeEvent(string title) => throw new NotImplementedException();

        public User FindUser(long id) => throw new NotImplementedException();

        public User FindUser(string usernameOrEmail) => throw new NotImplementedException();

        public void NotifyUser(Event @event, User user) => throw new NotImplementedException();

        public void NotifyUsers(Event @event, List<User> user) => throw new NotImplementedException();
    }
}

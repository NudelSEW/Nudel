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
        private IMongoCollection<User> userCollection;
        private IMongoCollection<Event> eventCollection;

        public NudelService()
        {
            mongo = new MongoClient("mongodb://nudel:nudel@docker:27017");
            db = mongo.GetDatabase("nudel");
            userCollection = db.GetCollection<User>("users");
            eventCollection = db.GetCollection<Event>("events");
        }

        public string Register(string username, string email, string password, string firstName, string lastName)
        {
            long id = userCollection.Count(x=>true) + 1;

            var results = userCollection.Find(x => x.Username == username || x.Email == email);

            if (results.Count() == 0)
                userCollection.InsertOne(new User
                {
                    ID = id,
                    Username = username,
                    Email = email,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName
                });
            else
            {
                return "error";
            }
            return "1234";
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public string Login(string usernameOrEmail, string password)
        {
            var collection = db.GetCollection<User>("users");
            var results = collection.Find(x => x.Username == usernameOrEmail && x.Password == password || x.Email == usernameOrEmail && x.Password == password);
            if (results.Count() > 0)
            {
                Console.WriteLine("you are logged in");
            }
            else
                Console.WriteLine("You are a FAILURE");

                return "123";
        }

        public void CreateEvent(string title, string description, DateTime time, Location location, List<DateTime> options)
        {
            long id = eventCollection.Count(x => true) + 1;

            eventCollection.InsertOne(new Event
            {
                ID = id,
                Title = title,
                Description = description,
                Time = time,
                Location = location,
                Options = options
            });
        }

        public Event FindEvent(long id)
        {
            var result = eventCollection.Find(x => x.ID == id);

            if (result.Count() != 1)
            {
                return null;
            }
            return result.First();
        }

        public List<Event> FindEvents(string title) => throw new NotImplementedException();

        public void SubscribeEvent(Event @event) => throw new NotImplementedException();

        public void SubscribeEvent(long id) => throw new NotImplementedException();

        public void SubscribeEvent(string title) => throw new NotImplementedException();

        public User FindUser(long id)
        {
            var collection = db.GetCollection<User>("users");
            var result = collection.Find(x => x.ID == id);
            if (result.Count() != 1)
            {
                return null;
            }
            else
            {
                return result.First();
            }
        }
        public User FindUser(string usernameOrEmail)
        {
            var collection = db.GetCollection<User>("users");
            var result = collection.Find(x => x.Username == usernameOrEmail || x.Email == usernameOrEmail);
            if (result.Count() != 1)
            {
                return null;
            }
            else
            {
                return result.First();
            }
        }

        public void NotifyUser(Event @event, User user) => throw new NotImplementedException();

        public void NotifyUsers(Event @event, List<User> user) => throw new NotImplementedException();
    }
}

using MongoDB.Driver;
using Nudel.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

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
            long id = userCollection.Count(x => true) + 1;

            var results = userCollection.Find(x => x.Username == username || x.Email == email);

            if (results.Count() == 0) {
                User user = new User
                {
                    ID = id,
                    Username = username,
                    Email = email,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName
                };

                userCollection.InsertOne(user);

                return CreateSessionToken(user);
            }
            else
            {
                return "{error:'passwordInvalid'}";
            }
        }

        public string Login(string usernameOrEmail, string password)
        {
            var collection = db.GetCollection<User>("users");
            var results = collection.Find(x => x.Username == usernameOrEmail && x.Password == password || x.Email == usernameOrEmail && x.Password == password);

            if (results.Count() > 0)
            {
                return CreateSessionToken(results.FirstOrDefault());
            }

            return "{error:'passwordInvalid'";
        }

        public void Logout(string usernameOrEmail, string password)
        {
            userCollection.UpdateOne(
                x => x.Username == usernameOrEmail || x.Email == usernameOrEmail,
                Builders<User>.Update.Set(x => x.SessionToken, "")
            );
        }

        public void CreateEvent(string sessionToken, Event @event)
        {
            User user = CheckSessionToken(sessionToken);

            if (user != null)
            {
                long id = eventCollection.Count(x => true) + 1;

                @event.Owner = user;

                eventCollection.InsertOne(@event);
            }
        }

        public Event FindEvent(string sessionToken, long id)
        {
            User user = CheckSessionToken(sessionToken);

            if (user != null)
            {
                var result = eventCollection.Find(x => x.ID == id);

                if (result.Count() != 1)
                {
                    return null;
                }

                return result.FirstOrDefault();
            }

            return null;
        }

        public List<Event> FindEvents(string sessionToken, string title)
        {
            User user = CheckSessionToken(sessionToken);

            if (user != null)
            {
                var result = eventCollection.Find(x => x.Title == title);

                if (result.Count() == 0)
                {
                    return null;
                }
                return result.ToList();
            }

            return null;
        }

        public void SubscribeEvent(string sessionToken, Event @event) => throw new NotImplementedException();

        public void SubscribeEvent(string sessionToken, long id) => throw new NotImplementedException();

        public void SubscribeEvent(string sessionToken, string title) => throw new NotImplementedException();

        public User FindUser(string sessionToken, long id)
        {
            User user = CheckSessionToken(sessionToken);

            if (user != null)
            {
                var result = userCollection.Find(x => x.ID == id);

                if (result.Count() != 1)
                {
                    return null;
                }
                return result.FirstOrDefault();
            }

            return null;
        }

        public User FindUser(string sessionToken, string usernameOrEmail)
        {
            User user = CheckSessionToken(sessionToken);

            if (user != null)
            {
                var result = userCollection.Find(x => x.Username == usernameOrEmail || x.Email == usernameOrEmail);

                if (result.Count() != 1)
                {
                    return null;
                }
                return result.FirstOrDefault();
            }

            return null;
        }

        public void NotifyUser(Event @event, User user) => throw new NotImplementedException();

        public void NotifyUsers(Event @event, List<User> user) => throw new NotImplementedException();

        private string CreateSessionToken(User user)
        {
            string hash = CreateRandomString(64);
            string sessionToken = $"{user.Username}-{hash}";

            user.SessionToken = sessionToken;

            userCollection.UpdateOne(x => x.ID == user.ID, Builders<User>.Update.Set(x => x.SessionToken, user.SessionToken));

            return sessionToken;
        }

        private User CheckSessionToken(string sessionToken)
        {
            var result = userCollection.Find(x => x.SessionToken == sessionToken);

            if (result.Count() == 1)
            {
                return result.FirstOrDefault();
            }

            return null;
        }

        private static string CreateRandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

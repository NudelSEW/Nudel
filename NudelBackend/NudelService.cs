using MongoDB.Bson;
using MongoDB.Driver;
using Nudel.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nudel.Backend
{
    /// <summary>
    /// The NudelService class contains all major functionality to interact with the database. 
    /// With it you can create, delete and query for Events and Users.
    /// </summary>
    public class NudelService
    {
        private static MongoClient mongo;
        private static IMongoDatabase db;
        private static IMongoCollection<User> userCollection;
        private static IMongoCollection<Event> eventCollection;

        private User user;

        /// <summary>
        /// The static NudelService constructor initilizes the database connection.
        /// </summary>
        static NudelService()
        {
            mongo = new MongoClient("mongodb://nudel:nudel@docker:27017");
            db = mongo.GetDatabase("nudel");
            userCollection = db.GetCollection<User>("users");
            eventCollection = db.GetCollection<Event>("events");
        }

        /// <summary>
        /// The basic NudelService constructor creates the NudelService object.
        /// </summary>
        public NudelService() { }

        /// <summary>
        /// The basic NudelService constructor creates the NudelService object.
        /// </summary>
        public NudelService(string sessionToken)
        {
            user = ValidateSessionToken(sessionToken);
            CheckSessionTokenProvided();
        }

        #region Authentication

        /// <summary>
        /// Registers a new User and returns a temporary Session Token
        /// </summary>
        /// <param name="username">Username of the new User</param>
        /// <param name="email">Email of the new User</param>
        /// <param name="password">Password of the new User</param>
        /// <param name="firstName">First Name of the new User</param>
        /// <param name="lastName">LastName of the new User</param>
        /// <returns></returns>
        public string Register(string username, string email, string password, string firstName, string lastName)
        {
            var results = userCollection.Find(x => x.Username == username || x.Email == email);

            if (results.Count() == 0) {
                User user = new User
                {
                    ID = ObjectId.GenerateNewId(),
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

        /// <summary>
        /// Logs in a already persistent User and returns a temporary Session Token
        /// </summary>
        /// <param name="usernameOrEmail">Username or Email of the persistent User</param>
        /// <param name="password">Password of the persistent User</param>
        /// <returns></returns>
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

        public void Logout(string sessionToken)
        {
            userCollection.UpdateOne(
                x => x.SessionToken == sessionToken,
                Builders<User>.Update.Set(x => x.SessionToken, "")
            );
        }

        #endregion

        #region Events

        public void CreateEvent(Event @event)
        {
            CheckSessionTokenProvided();

            @event.ID = ObjectId.GenerateNewId();
            @event.Owner = user;

            eventCollection.InsertOne(@event);
        }

        public void EditEvent(Event newEvent) => throw new NotImplementedException();

        public void DeleteEvent(Event @event) => throw new NotImplementedException();

        public Event FindEvent(ObjectId id)
        {
            CheckSessionTokenProvided();

            var result = eventCollection.Find(x => x.ID == id);

            if (result.Count() != 1)
            {
                return null;
            }

            return result.FirstOrDefault();
        }

        public List<Event> FindEvents(string title)
        {
            CheckSessionTokenProvided();

            var result = eventCollection.Find(x => x.Title == title);

            if (result.Count() == 0)
            {
                return null;
            }
            return result.ToList();
        }

        public void InviteToEvent(Event @event, User user)
        {
            user.Invitations.Add(@event);

            userCollection.ReplaceOne(x => x.ID == user.ID, user);
        }

        public void AcceptEvent(Event @event)
        {
            if (user.Invitations.Any(x => x.ID == @event.ID))
            {
                user.Invitations.Remove(@event);
                user.JoinedEvents.Add(@event);

                userCollection.ReplaceOne(x => x.ID == user.ID, user);
            }
        }

        public void LeaveEvent(Event @event)
        {
            if (user.JoinedEvents.Any(x => x.ID == @event.ID))
            {
                user.JoinedEvents.Remove(@event);

                userCollection.ReplaceOne(x => x.ID == user.ID, user);
            }
        }

        #endregion

        #region Users

        public User FindUser(ObjectId id)
        {
            CheckSessionTokenProvided();

            return userCollection.Find(x => x.ID == id).FirstOrDefault();
        }

        public User FindUser(string usernameOrEmail)
        {
            CheckSessionTokenProvided();

            var result = userCollection.Find(x => x.Username == usernameOrEmail || x.Email == usernameOrEmail);

            if (result.Count() != 1)
            {
                return null;
            }
            return result.FirstOrDefault();
        }

        public void EditUser(User newUser) => throw new NotImplementedException();

        public void DeleteUser(User user) => throw new NotImplementedException();

        #endregion

        #region Utilities

        private string CreateSessionToken(User user)
        {
            string hash = CreateRandomString(64);
            string sessionToken = $"{user.Username}-{hash}";

            user.SessionToken = sessionToken;

            userCollection.UpdateOne(x => x.ID == user.ID, Builders<User>.Update.Set(x => x.SessionToken, user.SessionToken));

            return sessionToken;
        }

        private User ValidateSessionToken(string sessionToken)
        {
            var result = userCollection.Find(x => x.SessionToken == sessionToken);

            if (result.Count() == 1)
            {
                return result.FirstOrDefault();
            }

            return null;
        }

        private void CheckSessionTokenProvided()
        {
            if (user == null)
            {
                throw new InvalidSessionTokenException("The session token provided is not valid");
            }
        }

        private static string CreateRandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion
    }
}

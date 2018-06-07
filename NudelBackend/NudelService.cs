﻿using MongoDB.Driver;
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

            long id = eventCollection.Count(x => true) + 1;

            @event.Owner = user;

            eventCollection.InsertOne(@event);
        }

        public void EditEvent(Event newEvent) => throw new NotImplementedException();

        public void EditEvent(long id, Event newEvent) => throw new NotImplementedException();

        public void DeleteEvent(Event @event) => throw new NotImplementedException();

        public void DeleteEvent(long id) => throw new NotImplementedException();

        public Event FindEvent(long id)
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

        }

        public void InviteToEvent(long eventId, User user) => throw new NotImplementedException();

        public void InviteToEvent(Event @event, long userId) => throw new NotImplementedException();

        public void InviteToEvent(long eventId, long userId) => throw new NotImplementedException();

        #endregion

        #region Users

        public User FindUser(long id)
        {
            CheckSessionTokenProvided();

            var result = userCollection.Find(x => x.ID == id);

            if (result.Count() != 1)
            {
                return null;
            }
            return result.FirstOrDefault();
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

        public void NotifyUser(Event @event, User user) => throw new NotImplementedException();

        public void NotifyUsers(Event @event, List<User> user) => throw new NotImplementedException();

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

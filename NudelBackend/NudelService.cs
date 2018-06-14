using MongoDB.Bson;
using MongoDB.Driver;
using Nudel.BusinessObjects;
using NudelBusinessObjects;
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
                    ID = Guid.NewGuid().ToString(),
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
            var results = userCollection.Find(x =>
                (x.Username == usernameOrEmail && x.Password == password) ||
                (x.Email == usernameOrEmail && x.Password == password)
            );

            if (results.Count() > 0)
            {
                return CreateSessionToken(results.FirstOrDefault());
            }

            return "{error:'passwordInvalid'";
        }

        /// <summary>
        /// updating the userCollection of the database to end the session and log out the user
        /// </summary>
        /// <param name="sessionToken"> individual session token for every session </param>
        public void Logout(string sessionToken)
        {
            userCollection.UpdateOne(
                x => x.SessionToken == sessionToken,
                Builders<User>.Update.Set(x => x.SessionToken, "")
            );
        }

        #endregion

        #region Events

        /// <summary>
        /// creating an event via id, owner and inserting the event into the database
        /// </summary>
        /// <param name="event"></param>
        public void CreateEvent(Event @event)
        {
            CheckSessionTokenProvided();

            @event.ID = Guid.NewGuid().ToString();
            @event.Owner = user;

            eventCollection.InsertOne(@event);
        }

        /// <summary>
        /// throws NotImplementedException
        /// </summary>
        /// <param name="newEvent">the new event attributes </param>
        public void EditEvent(Event newEvent) => throw new NotImplementedException();

        /// <summary>
        /// throws NotImplementedException
        /// </summary>
        /// <param name="event"> the event that should be deleted </param>
        public void DeleteEvent(Event @event) => throw new NotImplementedException();

        /// <summary>
        /// searching the event in the collection of the database
        /// </summary>
        /// <param name="id"> session id of the event </param>
        /// <returns> returns either the found event or null </returns>
        public Event FindEvent(string id)
        {
            CheckSessionTokenProvided();

            var result = eventCollection.Find(x => x.ID == id);

            if (result.Count() != 1)
            {
                return null;
            }

            return result.FirstOrDefault();
        }

        /// <summary>
        /// searching for multiple events in the collection of the database
        /// </summary>
        /// <param name="title"> the title of the events </param>
        /// <returns> returns either a list of events or null </returns>
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

        /// <summary>
        /// adds an existing user to an event and save it into the database
        /// </summary>
        /// <param name="event"> the new event for the user </param>
        /// <param name="user"> the new user </param>
        public void InviteToEvent(Event @event, User user)
        {
            user.Invitations.Add(@event);

            userCollection.ReplaceOne(x => x.ID == user.ID, user);
        }

        /// <summary>
        /// compares user and event id if its equal, if not invitation is sent
        /// </summary>
        /// <param name="event"> the event for the invitation </param>
        public void AcceptEvent(Event @event)
        {
            if (user.Invitations.Any(x => x.ID == @event.ID))
            {
                user.Invitations.Remove(@event);
                user.JoinedEvents.Add(@event);

                userCollection.ReplaceOne(x => x.ID == user.ID, user);
            }
        }


        /// <summary>
        /// compares if a user does participate in this event, if true then he gets removed
        /// </summary>
        /// <param name="event"></param>
        public void LeaveEvent(Event @event)
        {
            if (user.JoinedEvents.Any(x => x.ID == @event.ID))
            {
                user.JoinedEvents.Remove(@event);

                userCollection.ReplaceOne(x => x.ID == user.ID, user);
            }
        }

        /// <summary>
        /// throws NotImplementedException
        /// </summary>
        /// <param name="event"></param>
        /// <param name="comment"></param>
        public void AddComment(Event @event, Comment comment) => throw new NotImplementedException();

        /// <summary>
        /// throws NotImplementedException
        /// </summary>
        /// <param name="event"></param>
        /// <param name="comment"></param>
        public void DeleteComment(Event @event, Comment comment) => throw new NotImplementedException();

        #endregion

        #region Users

        /// <summary>
        /// searches in userCollection of the database and returns the user
        /// </summary>
        /// <returns> returns the existing user </returns>
        public User GetUser()
        {
            CheckSessionTokenProvided();

            return userCollection.Find(x => x.ID == user.ID).FirstOrDefault();
        }

        /// <summary>
        /// checks if the sessionToken is provided and searches users by id
        /// </summary>
        /// <param name="id"> the string id of user </param>
        /// <returns> returns the first matching user </returns>
        public User FindUserById(string id)
        {
            CheckSessionTokenProvided();

            return userCollection.Find(x => x.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// checks if the sessionToken is provided and searches users by usernameOrEmail
        /// </summary>
        /// <param name="usernameOrEmail"> the corresponding username or email </param>
        /// <returns> returns the first matching user or null </returns>
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

        /// <summary>
        /// throws NotImplementedException
        /// </summary>
        /// <param name="newUser"></param>
        public void EditUser(User newUser) => throw new NotImplementedException();

        /// <summary>
        /// throws NotImplementedException
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(User user) => throw new NotImplementedException();

        #endregion

        #region Utilities

        /// <summary>
        /// creates a random string,uses it to create a sessionToken and updates the userCollection
        /// </summary>
        /// <param name="user"> the matching user  </param>
        /// <returns> returns the generated sessionToken </returns>
        private string CreateSessionToken(User user)
        {
            string hash = CreateRandomString(64);
            string sessionToken = $"{user.Username}-{hash}";

            user.SessionToken = sessionToken;

            userCollection.UpdateOne(x => x.ID == user.ID, Builders<User>.Update.Set(x => x.SessionToken, user.SessionToken));

            return sessionToken;
        }

        /// <summary>
        /// searches for the given sessionToken in the collection 
        /// </summary>
        /// <param name="sessionToken"> validation needed sessionToken </param>
        /// <returns> returns the first matching value or null </returns>
        private User ValidateSessionToken(string sessionToken)
        {
            var result = userCollection.Find(x => x.SessionToken == sessionToken);

            if (result.Count() == 1)
            {
                return result.FirstOrDefault();
            }

            return null;
        }
       
        /// <summary>
        /// throws InvalidSessionTokenException if user is null
        /// </summary>
        private void CheckSessionTokenProvided()
        {
            if (user == null)
            {
                throw new InvalidSessionTokenException("The session token provided is not valid");
            }
        }

        /// <summary>
        /// creates a random object and returns an array of random chars 
        /// </summary>
        /// <param name="length"> the length of the array </param>
        /// <returns> retuns a new random string </returns>
        private static string CreateRandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion
    }
}

using MongoDB.Driver;
using Nudel.BusinessObjects;
using Nudel.Networking;
using System;
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
        public Result Register(User user)
        {
            var results = userCollection.Find(x => x.Username == user.Username || x.Email == user.Email);

            if (results.Count() == 0) {

                user.ID = Guid.NewGuid().ToString();

                userCollection.InsertOne(user);

                return new Result
                {
                    Type = ResultType.Success,
                    SessionToken = CreateSessionToken(user)
                };
            }

            return Result.Error(100, "Invalid User");
        }

        /// <summary>
        /// Logs in a already persistent User and returns a temporary Session Token
        /// </summary>
        /// <param name="usernameOrEmail">Username or Email of the persistent User</param>
        /// <param name="password">Password of the persistent User</param>
        /// <returns></returns>
        public Result Login(User user)
        {
            var results = userCollection.Find(x =>
                (x.Username == user.Username && x.Password == user.Password) ||
                (x.Email == user.Email && x.Password == user.Password)
            );

            if (results.Count() > 0)
            {
                return new Result {
                    Type = ResultType.Success,
                    SessionToken = CreateSessionToken(results.FirstOrDefault())
                };
            }

            return Result.Error(100, "Invalid User");
        }

        /// <summary>
        /// updating the userCollection of the database to end the session and log out the user
        /// </summary>
        /// <param name="sessionToken"> individual session token for every session </param>
        public Result Logout(string sessionToken)
        {
            userCollection.UpdateOne(
                x => x.SessionToken == sessionToken,
                Builders<User>.Update.Set(x => x.SessionToken, "")
            );

            return Result.Success();
        }

        #endregion

        #region Events

        /// <summary>
        /// creating an event via id, owner and inserting the event into the database
        /// </summary>
        /// <param name="event"></param>
        /// </summary>
        /// <param name="event">Event to be created</param>
        public Result CreateEvent(Event @event)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            @event.ID = Guid.NewGuid().ToString();
            @event.Owner = user;

            eventCollection.InsertOne(@event);

            return Result.Success();
        }

        /// <summary>
        /// Edits a event
        /// </summary>
        /// <param name="newEvent">Event to be edited</param>
        public Result EditEvent(Event newEvent)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            var foundEvents = eventCollection.Find(x => x.ID == newEvent.ID);

            if (foundEvents.Count() != 1)
            {
                return Result.Error(100, "Invalid event: Event doesn't exist");
            }

            Event oldEvent = foundEvents.FirstOrDefault();

            if (oldEvent.Owner != user)
            {
                return Result.Error(500, "User is not owner of event");
            }

            eventCollection.ReplaceOne(x => x.ID == oldEvent.ID, newEvent);

            return Result.Success();
        }

        /// <summary>
        /// Deletes a event
        /// </summary>
        /// <param name="event"> the event that should be deleted </param>
        public Result DeleteEvent(Event @event)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            var foundEvents = eventCollection.Find(x => x.ID == @event.ID);

            if (foundEvents.Count() != 1)
            {
                return Result.Error(100, "Invalid event: Event doesn't exist");
            }

            Event foundEvent = foundEvents.FirstOrDefault();

            if (foundEvent.Owner != user)
            {
                return Result.Error(500, "User is not owner of event");
            }

            eventCollection.DeleteOne(x => x.ID == foundEvent.ID);

            return Result.Success();
        }

        /// <summary>
        /// searching the event in the collection of the database
        /// </summary>
        /// <param name="id"> id of the event </param>
        /// <returns> returns either the found event or null </returns>
        public Result FindEvent(string id)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            var foundEvents = eventCollection.Find(x => x.ID == id);

            if (foundEvents.Count() != 1)
            {
                return Result.Error(200, "Event not found");
            }

            return new Result
            {
                Type = ResultType.Success,
                FoundEvent = foundEvents.FirstOrDefault()
            };
        }

        /// <summary>
        /// searching for multiple events in the collection of the database
        /// </summary>
        /// <param name="title"> the title of the events </param>
        /// <returns> returns either a list of events or null </returns>
        public Result FindEvents(string title)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            var foundEvents = eventCollection.Find(x => x.Title == title);

            if (foundEvents.Count() == 0)
            {
                return Result.Error(200, "Event not found");
            }

            return new Result
            {
                Type = ResultType.Success,
                FoundEvents = foundEvents.ToList()
            };
        }

        /// <summary>
        /// adds an existing user to an event and save it into the database
        /// </summary>
        /// <param name="event"> the new event for the user </param>
        /// <param name="user"> the new user </param>
        public Result InviteToEvent(Event @event, User user)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            user.Invitations.Add(@event);

            userCollection.ReplaceOne(x => x.ID == user.ID, user);

            return Result.Success();
        }

        /// <summary>
        /// compares user and event id if its equal, if not invitation is sent
        /// </summary>
        /// <param name="event"> the event for the invitation </param>
        public Result AcceptEvent(Event @event)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            if (user.Invitations.Any(x => x.ID == @event.ID))
            {
                user.Invitations.Remove(@event);
                user.JoinedEvents.Add(@event);

                userCollection.ReplaceOne(x => x.ID == user.ID, user);
            }

            return Result.Success();
        }

        /// <summary>
        /// compares if a user does participate in this event, if true then he gets removed
        /// </summary>
        /// <param name="event"></param>
        public Result LeaveEvent(Event @event)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            if (user.JoinedEvents.Any(x => x.ID == @event.ID))
            {
                user.JoinedEvents.Remove(@event);

                userCollection.ReplaceOne(x => x.ID == user.ID, user);
            }

            return Result.Success();
        }

        /// <summary>
        /// Adds a comment to a event
        /// </summary>
        /// <param name="event"> event, the comment should be added to</param>
        /// <param name="comment">comment, that should be added</param>
        public Result AddComment(Event @event, Comment comment)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            if (!@event.Members.Any(x => x == user))
            {
                return Result.Error(500, "User is not member of event");
            }

            @event.Comments.Add(comment);

            eventCollection.ReplaceOne(x => x.ID == @event.ID, @event);

            return Result.Success();
        }

        /// <summary>
        /// Deletes a comment to a event
        /// </summary>
        /// <param name="event"> event, the comment should be deleted from</param>
        /// <param name="comment">comment, that should be deleted</param>
        public Result DeleteComment(Event @event, Comment comment)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            if (@event.Owner != user)
            {
                return Result.Error(500, "User is not owner of event");
            }

            @event.Comments.Remove(comment);

            eventCollection.ReplaceOne(x => x.ID == @event.ID, @event);

            return Result.Success();
        }

        #endregion

        #region Users

        /// <summary>
        /// searches in userCollection of the database and returns the user
        /// </summary>
        /// <returns> returns the existing user </returns>
        public Result FindCurrentUser()
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            return new Result
            {
                Type = ResultType.Success,
                FoundUser = userCollection.Find(x => x.ID == user.ID).FirstOrDefault()
            };
        }

        /// <summary>
        /// checks if the sessionToken is provided and searches users by id
        /// </summary>
        /// <param name="id"> the string id of user </param>
        /// <returns> returns the first matching user </returns>
        public Result FindUserById(string id)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            User foundUser = userCollection.Find(x => x.ID == id).FirstOrDefault();

            if (foundUser != null)
            {
                return Result.Error(300, "User not found");
            }

            return new Result
            {
                Type = ResultType.Success,
                FoundUser = userCollection.Find(x => x.ID == user.ID).FirstOrDefault()
            };
        }

        /// <summary>
        /// checks if the sessionToken is provided and searches users by usernameOrEmail
        /// </summary>
        /// <param name="usernameOrEmail"> the corresponding username or email </param>
        /// <returns> returns the first matching user or null </returns>
        public Result FindUser(string usernameOrEmail)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            var foundUsers = userCollection.Find(x => x.Username == usernameOrEmail || x.Email == usernameOrEmail);

            if (foundUsers.Count() != 1)
            {
                return null;
            }

            return new Result
            {
                Type = ResultType.Success,
                FoundUser = foundUsers.FirstOrDefault()
            };
        }

        /// <summary>
        /// Edits a user
        /// </summary>
        /// <param name="newUser"> new user to be changed</param>
        public Result EditUser(User newUser)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            var foundUsers = userCollection.Find(x => x.ID == newUser.ID);

            if (foundUsers.Count() != 1)
            {
                return Result.Error(100, "Invalid user: User doesn't exist");
            }

            User oldUser = foundUsers.FirstOrDefault();

            if (oldUser != user)
            {
                return Result.Error(600, "User is permitted to edit another user");
            }

            userCollection.ReplaceOne(x => x.ID == oldUser.ID, newUser);

            return Result.Success();
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="user"> user to be deleted </param>
        public Result DeleteUser(User user)
        {
            Result result = CheckSessionTokenProvided();
            if (result.Type == ResultType.Error) return result;

            var foundUsers = userCollection.Find(x => x.ID == user.ID);

            if (foundUsers.Count() != 1)
            {
                return Result.Error(100, "Invalid user: User doesn't exist");
            }

            User foundUser = foundUsers.FirstOrDefault();

            if (foundUser != user)
            {
                return Result.Error(500, "User is not permitted to delete another user");
            }

            userCollection.DeleteOne(x => x.ID == user.ID);

            return Result.Success();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// creates a random string, uses it to create a sessionToken and updates the userCollection
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
        private Result CheckSessionTokenProvided()
        {
            if (user == null)
            {
                return Result.Error(400, "Invalid Session Token");
            }

            return Result.Success();
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

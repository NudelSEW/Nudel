using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Nudel.BusinessObjects
{
    public class User
    {
        [BsonId]
        public long ID { get; set; }
        [BsonElement("username")]
        public string Username { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("firstName")]
        public string FirstName { get; set; }
        [BsonElement("lastName")]
        public string LastName { get; set; }
        [BsonElement("ownedEvents")]
        public List<Event> OwnedEvents { get; set; }
        [BsonElement("sessionToken")]
        public string SessionToken { get; set; }

        public User() { }
        public User(long id, string username, string email, string password, string firstName, string lastName, List<Event> ownedEvents)
        {
            ID = id;
            Username = username;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            OwnedEvents = ownedEvents;
        }
    }
}

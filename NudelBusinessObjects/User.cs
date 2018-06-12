using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Nudel.BusinessObjects
{
    public class User
    {
        [BsonId]
        public string ID { get; set; }
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
        [BsonElement("joinedEvents")]
        public List<Event> JoinedEvents { get; set; }
        [BsonElement("invitations")]
        public List<Event> Invitations { get; set; }
        [BsonElement("sessionToken")]
        public string SessionToken { get; set; }

        public User() { }

        public User(string username, string email, string password, string firstName, string lastName)
        {
            Username = username;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}

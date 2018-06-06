using System.Collections.Generic;

namespace Nudel.Backend.BusinessObjects
{
    public class User
    {
        public long ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Event> OwnedEvents { get; set; }

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

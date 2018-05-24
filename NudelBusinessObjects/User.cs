using System.Collections.Generic;

namespace Nudel.BusinessObjects
{
    public class User
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public List<Event> OwnedEvents { get; set; }

        public User() { }
        public User(long id, string firstName, string secondName, string email, List<Event> ownedEvents)
        {
            ID = id;
            FirstName = firstName;
            SecondName = secondName;
            Email = email;
            OwnedEvents = ownedEvents;
        }
    }
}

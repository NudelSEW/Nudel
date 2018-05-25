using System;
using System.Collections.Generic;

namespace Nudel.BusinessObjects
{
    public class Event
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public Location Location { get; set; }
        public User Owner { get; set; }
        public List<User> Members { get; set; }
        public List<DateTime> Options { get; set; }

        public Event() { }
        public Event(
            long id,
            string title,
            string description,
            DateTime time,
            Location location,
            User owner,
            List<User> members,
            List<DateTime> options
        )
        {
            ID = id;
            Title = title;
            Description = description;
            Time = time;
            Location = location;
            Owner = owner;
            Members = members;
            Options = options;
        }
    }
}

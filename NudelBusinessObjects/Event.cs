using System;
using System.Collections.Generic;

namespace Nudel.BusinessObjects
{
    public class Event
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public Location Location { get; set; }
        public User Owner { get; set; }
        public List<User> Members { get; set; }
        public List<DateTime> Options { get; set; }

        public Event() { }
        public Event(
            string title,
            string description,
            DateTime time,
            Location location,
            User owner,
            List<User> members,
            List<DateTime> options
        )
        {
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

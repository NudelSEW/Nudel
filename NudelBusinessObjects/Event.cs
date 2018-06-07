using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Nudel.BusinessObjects
{
    public class Event
    {
        [BsonId]
        public long ID { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("descriptions")]
        public string Description { get; set; }
        [BsonElement("time")]
        public DateTime Time { get; set; }
        [BsonElement("location")]
        public Location Location { get; set; }
        [BsonElement("owner")]
        public User Owner { get; set; }
        [BsonElement("members")]
        public List<User> Members { get; set; }
        [BsonElement("options")]
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

using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nudel.BusinessObjects
{
    public class Event
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public GeoCoordinate Location { get; set; }
        public string Description { get; set; }
        public User Owner { get; set; }
        public List<User> Members { get; set; }
        public List<DateTime> Options { get; set; }

        public void NotifyUsers()
        {

        }

        public void EndPoll()
        {

        }
    }
}

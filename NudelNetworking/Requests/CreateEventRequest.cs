using Nudel.BusinessObjects;
using Nudel.Networking.Requests.Base;
using System;
using System.Collections.Generic;

namespace Nudel.Networking.Requests
{
    public class CreateEventRequest : AuthenticatedRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public Location Location { get; set; }
        public List<DateTime> Options { get; set; }

        public CreateEventRequest() { }

        /// <summary>
        ///  using the entered parameters to create a request 
        /// </summary>
        /// <param name="sessionToken"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="time"></param>
        /// <param name="location"></param>
        /// <param name="options"></param>
        public CreateEventRequest(
            string sessionToken,
            string title,
            string description,
            DateTime time,
            Location location,
            List<DateTime> options
        ) : base(sessionToken)
        {
            Title = title;
            Description = description;
            Time = time;
            Location = location;
            Options = options;
        }
    }
}

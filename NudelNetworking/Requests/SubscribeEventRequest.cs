using System;
using System.Collections.Generic;
using Nudel.Networking.Requests.Base;
using Nudel.BusinessObjects;
using System.Text;

namespace Nudel.Networking.Requests
{
    class SubscribeEventRequest : AuthenticatedRequest
    {
        public long ID { get; set; }
        public string Titel { get; set; }
        public Event Event{ get; set; }
        public SubscribeEventRequest() : base() { }

        public SubscribeEventRequest(string sessionToken, long id) : base(sessionToken)
        {
            ID = id;
        }

        public SubscribeEventRequest(string sessionToken, string titel) : base(sessionToken)
        {
            Titel = titel;
        }

        public SubscribeEventRequest(string sessionToken, Event @event) : base(sessionToken)
        {
            Event = @event;
        }
    }
}

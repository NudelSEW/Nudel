using System;
using System.Collections.Generic;
using Nudel.Networking.Requests.Base;
using Nudel.BusinessObjects;
using System.Text;

namespace Nudel.Networking.Requests
{
    class InviteToEventRequest : AuthenticatedRequest
    {
        public long ID { get; set; }
        public string Titel { get; set; }
        public Event Event{ get; set; }
        public InviteToEventRequest() : base() { }

        public InviteToEventRequest(string sessionToken, long id) : base(sessionToken)
        {
            ID = id;
        }

        public InviteToEventRequest(string sessionToken, string titel) : base(sessionToken)
        {
            Titel = titel;
        }

        public InviteToEventRequest(string sessionToken, Event @event) : base(sessionToken)
        {
            Event = @event;
        }
    }
}

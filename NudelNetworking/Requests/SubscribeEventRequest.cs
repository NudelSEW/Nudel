using System;
using System.Collections.Generic;
using Nudel.Networking.Requests.Base;
using Nudel.BusinessObjects;
using System.Text;

namespace Nudel.Networking.Requests
{   
    /// <summary>
    /// storing values of the event invitation 
    /// </summary>
    class InviteToEventRequest : AuthenticatedRequest
    {
        public long ID { get; set; }
        public string Titel { get; set; }
        public Event Event{ get; set; }
        public InviteToEventRequest() : base() { }

        /// <summary>
        /// taking the sessionToken and id of the event and setting it equals to the id
        /// </summary>
        /// <param name="sessionToken"> string of the event </param>
        /// <param name="id"> identity of the event </param>
        public InviteToEventRequest(string sessionToken, long id) : base(sessionToken)
        {
            ID = id;
        }

        /// <summary>
        /// setting the parameter title equal to the id.
        /// </summary>
        /// <param name="sessionToken">string for authentification</param>
        /// <param name="titel">the title of the specific event</param>
        public InviteToEventRequest(string sessionToken, string titel) : base(sessionToken)
        {
            Titel = titel;
        }

        /// <summary>
        /// setting the parameter sessionToken and @event 
        /// </summary>
        /// <param name="sessionToken"></param>
        /// <param name="event"></param>
        public InviteToEventRequest(string sessionToken, Event @event) : base(sessionToken)
        {
            Event = @event;
        }
    }
}

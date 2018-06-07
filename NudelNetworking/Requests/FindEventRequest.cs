using System;
using System.Collections.Generic;
using System.Text;

namespace Nudel.Networking.Requests.Base
{
    class FindEventRequest : AuthenticatedRequest
    {
        public long ID { get; set; }
      
        public FindEventRequest() : base() { }

        public FindEventRequest(string sessionToken, long id) : base(sessionToken)
        {
            ID = id;
        }

       

    }
}

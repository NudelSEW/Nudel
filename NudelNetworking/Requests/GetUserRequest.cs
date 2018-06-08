using Nudel.Networking.Requests.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nudel.Networking.Requests
{
    public class GetUserRequest : AuthenticatedRequest
    {

        public GetUserRequest() : base() { }

        public GetUserRequest(string sessionToken) : base(sessionToken) { }
    }
}

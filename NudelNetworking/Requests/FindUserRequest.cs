using System;
using System.Collections.Generic;
using System.Text;

namespace Nudel.Networking.Requests.Base
{
    class FindUserRequest : AuthenticatedRequest
    {
        public long ID { get; set; }
        public string UsernameOrEmail { get; set; }

        public FindUserRequest() : base() { }

        public FindUserRequest(string sessionToken, long id) : base(sessionToken)
        {
            ID = id;
        }

        public FindUserRequest(string sessionToken, string usernameOrEmail) : base(sessionToken)
        {
            UsernameOrEmail = usernameOrEmail;
        }
    }
}

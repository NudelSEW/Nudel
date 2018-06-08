using Nudel.BusinessObjects;
using Nudel.Networking.Responses.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nudel.Networking.Responses
{
    public class GetUserResponse : Response
    {
        public User User { get; set; }

        public GetUserResponse(User user)
        {
            User = user;
        }
    }
}

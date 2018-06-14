using Nudel.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nudel.Networking
{
    public class Response
    {
        public RequestResponseType Type { get; set; }
        public Result Result { get; set; }

        public Response() { }
    }
}

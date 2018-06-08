using Nudel.BusinessObjects;
using Nudel.Networking.Responses.Base;

namespace Nudel.Networking.Responses
{
    public class FindUserResponse : Response
    {
        public User User { get; set; }

        public FindUserResponse() { }

        public FindUserResponse(User user) : base()
        {
            User = user;
        }
    }
}

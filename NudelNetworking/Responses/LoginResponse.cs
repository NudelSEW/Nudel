using Nudel.Networking.Responses.Base;

namespace Nudel.Networking.Responses
{
    public class LoginResponse : Response
    {
        public string SessionToken;

        public LoginResponse() { }
        public LoginResponse(string sessionToken) : base()
        {
            SessionToken = sessionToken;
        }
    }
}

using Nudel.Networking.Responses.Base;

namespace Nudel.Networking.Responses
{
    public class LoginRegisterResponse : Response
    {
        public string SessionToken;

        public LoginRegisterResponse() { }
        public LoginRegisterResponse(string sessionToken) : base()
        {
            SessionToken = sessionToken;
        }
    }
}

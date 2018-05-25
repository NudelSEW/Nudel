using Nudel.Networking.Requests.Base;

namespace Nudel.Networking.Requests
{
    public class AuthenticationRequest : NonAuthenticatedRequest
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }

        public AuthenticationRequest() : base() { }
        public AuthenticationRequest(string usernameOrEmail, string password) : base()
        {
            UsernameOrEmail = usernameOrEmail;
            Password = password;
        }
    }
}

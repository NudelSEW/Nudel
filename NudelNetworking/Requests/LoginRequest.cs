using Nudel.Networking.Requests.Base;

namespace Nudel.Networking.Requests
{
    public class LoginRequest : NonAuthenticatedRequest
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }

        public LoginRequest() : base() { }
        public LoginRequest(string usernameOrEmail, string password) : base()
        {
            UsernameOrEmail = usernameOrEmail;
            Password = password;
        }
    }
}

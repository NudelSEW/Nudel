namespace Nudel.Networking.Requests
{
    public class AuthenticationRequest : Request
    {
        public AuthenticationRequest() : base() { }
        public AuthenticationRequest(string usernameOrEmail, string password) : base(usernameOrEmail, password) { }
    }
}

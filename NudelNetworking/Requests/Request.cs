namespace Nudel.Networking.Requests
{
    public class Request
    {
        public string SessionToken;
        public string UsernameOrEmail;
        public string Password;

        public Request() { }
        public Request(string sessionToken)
        {
            SessionToken = sessionToken;
        }
        public Request(string usernameOrEmail, string password)
        {
            UsernameOrEmail = usernameOrEmail;
            Password = password;
        }
    }
}

namespace Nudel.Networking.Responses
{
    public class AuthenticationResponse
    {
        public string SessionToken;

        public AuthenticationResponse() { }
        public AuthenticationResponse(string sessionToken)
        {
            SessionToken = sessionToken;
        }
    }
}

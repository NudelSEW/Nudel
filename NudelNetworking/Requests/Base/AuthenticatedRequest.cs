namespace Nudel.Networking.Requests.Base
{
    public class AuthenticatedRequest
    {
        public string SessionToken { get; set; }

        public AuthenticatedRequest() { }
        public AuthenticatedRequest(string sessionToken)
        {
            SessionToken = sessionToken;
        }
    }
}

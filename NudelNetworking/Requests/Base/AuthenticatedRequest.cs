namespace Nudel.Networking.Requests.Base
{
    public class AuthenticatedRequest : Request
    {
        public string SessionToken { get; set; }

        public AuthenticatedRequest() : base() { }
        public AuthenticatedRequest(string sessionToken) : base()
        {
            SessionToken = sessionToken;
        }
    }
}

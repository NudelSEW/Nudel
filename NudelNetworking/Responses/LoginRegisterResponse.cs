using Nudel.Networking.Responses.Base;

namespace Nudel.Networking.Responses
{
    public class LoginRegisterResponse : Response
    {
        public string SessionToken;

        /// <summary>
        /// a default constructor
        /// </summary>
        public LoginRegisterResponse() { }
        /// <summary>
        /// constructor using the sessiontoken for response
        /// </summary>
        /// <param name="sessionToken"> a string for authentification</param>
        public LoginRegisterResponse(string sessionToken) : base()
        {
            SessionToken = sessionToken;
        }
    }
}

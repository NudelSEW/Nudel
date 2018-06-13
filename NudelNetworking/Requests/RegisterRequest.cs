using Nudel.Networking.Requests.Base;

namespace Nudel.Networking.Requests
{
    public class RegisterRequest : NonAuthenticatedRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public RegisterRequest() : base() { }

        /// <summary>
        /// setting parameters equal to base method of request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public RegisterRequest(string username, string email, string password, string firstName, string lastName) : base()
        {

            Username = username;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}

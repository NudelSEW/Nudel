using Nudel.BusinessObjects;
using Nudel.Networking.Responses.Base;

namespace Nudel.Networking.Responses
{
    public class FindUserResponse : Response
    {
        public User User { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public FindUserResponse() { }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="user">the Users data </param>
        public FindUserResponse(User user) : base()
        {
            User = user;
        }
    }
}

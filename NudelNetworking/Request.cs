using Nudel.BusinessObjects;

namespace Nudel.Networking
{
    public class Request
    {
        public RequestResponseType Type { get; set; }
        public string SessionToken { get; set; }
        public User User { get; set; }
        public Event Event { get; set; }
        public Comment Comment { get; set; }

        public Request() { }
    }
}

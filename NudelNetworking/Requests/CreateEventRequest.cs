using Nudel.BusinessObjects;

namespace Nudel.Networking.Requests
{
    public class CreateEventRequest : Request
    {
        public Event Event;

        public CreateEventRequest() { }
        public CreateEventRequest(
            string sessionToken,
            Event @event
        ) : base(sessionToken)
        {
            Event = @event;
        }
    }
}

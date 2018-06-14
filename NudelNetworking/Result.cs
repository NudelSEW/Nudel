using Nudel.BusinessObjects;
using System.Collections.Generic;

namespace Nudel.Networking
{
    public class Result
    {
        public ResultType Type { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string SessionToken { get; set; }
        public User FoundUser { get; set; }
        public Event FoundEvent { get; set; }
        public List<Event> FoundEvents { get; set; }

        public Result() { }

        public static Result Success()
        {
            return new Result { Type = ResultType.Success };
        }

        public static Result Error(int errorCode, string errorDescription)
        {
            return new Result {
                Type = ResultType.Error,
                ErrorCode = errorCode,
                ErrorDescription = errorDescription
            };
        }
    }
}

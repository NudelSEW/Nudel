using System;
using System.Runtime.Serialization;

namespace Nudel.Backend
{
    [Serializable]
    internal class InvalidSessionTokenException : Exception
    {
        public InvalidSessionTokenException()
        {
        }

        public InvalidSessionTokenException(string message) : base(message)
        {
        }

        public InvalidSessionTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidSessionTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace Nudel.Backend
{
    /// <summary>
    /// returning an exception if the sessionToken is invalid
    /// </summary>
    [Serializable]
    internal class InvalidSessionTokenException : Exception
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public InvalidSessionTokenException()
        {
        }

        /// <summary>
        /// constructor with a string message as parameter
        /// </summary>
        /// <param name="message"> the output message </param>
        public InvalidSessionTokenException(string message) : base(message)
        {
        }

        /// <summary>
        /// constructor with string message and Exception as parameter
        /// </summary>
        /// <param name="message"> the output message </param>
        /// <param name="innerException"> the thrown exception
        public InvalidSessionTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// constructor with serializationInfo and streaming context as parameter
        /// </summary>
        /// <param name="info"> informations about serialization </param>
        /// <param name="context"> the context of the stream </param>
        protected InvalidSessionTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
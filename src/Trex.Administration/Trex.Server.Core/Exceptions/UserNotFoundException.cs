using System;
using System.Runtime.Serialization;

namespace Trex.Server.Core.Exceptions
{
    /// <summary>
    /// Thrown when user was not found by the provided credentials
    /// </summary>
    [Serializable]
    public class UserNotFoundException : UserException
    {
        public UserNotFoundException() {}
        public UserNotFoundException(string message) : base(message) {}
        public UserNotFoundException(string message, Exception inner) : base(message, inner) {}

        public UserNotFoundException(string message, Exception inner, string userName) : base(message, inner)
        {
            UserName = userName;
        }

        protected UserNotFoundException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {}
    }
}
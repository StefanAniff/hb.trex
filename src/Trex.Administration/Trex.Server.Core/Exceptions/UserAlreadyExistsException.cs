using System;
using System.Runtime.Serialization;

namespace Trex.Server.Core.Exceptions
{
    [Serializable]
    public class UserAlreadyExistsException : UserException
    {
        public UserAlreadyExistsException() {}
        public UserAlreadyExistsException(string message) : base(message) {}
        public UserAlreadyExistsException(string message, Exception inner) : base(message, inner) {}

        public UserAlreadyExistsException(string message, Exception inner, string userName) : base(message, inner)
        {
            UserName = userName;
        }

        protected UserAlreadyExistsException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {}
    }
}
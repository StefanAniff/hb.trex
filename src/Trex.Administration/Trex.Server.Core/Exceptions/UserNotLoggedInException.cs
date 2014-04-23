using System;
using System.Runtime.Serialization;

namespace Trex.Server.Core.Exceptions
{
    [Serializable]
    public class UserNotLoggedInException : ApplicationBaseException
    {
        public UserNotLoggedInException() {}
        public UserNotLoggedInException(string message) : base(message) {}
        public UserNotLoggedInException(string message, Exception inner) : base(message, inner) {}

        protected UserNotLoggedInException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {}
    }
}
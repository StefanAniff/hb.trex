using System;

namespace Trex.Core.Exceptions
{
    public class UserNotLoggedInException : ApplicationBaseException
    {
        public UserNotLoggedInException() {}
        public UserNotLoggedInException(string message) : base(message) {}
        public UserNotLoggedInException(string message, Exception inner) : base(message, inner) {}
    }
}
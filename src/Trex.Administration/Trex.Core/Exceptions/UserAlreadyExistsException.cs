using System;

namespace Trex.Core.Exceptions
{
    public class UserAlreadyExistsException : UserException
    {
        public UserAlreadyExistsException() {}
        public UserAlreadyExistsException(string message) : base(message) {}
        public UserAlreadyExistsException(string message, Exception inner) : base(message, inner) {}

        public UserAlreadyExistsException(string message, Exception inner, string userName) : base(message, inner)
        {
            UserName = userName;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Exceptions
{
    /// <summary>
    /// Thrown when user was not found by the provided credentials
    /// </summary>
    [global::System.Serializable]
    public class UserNotFoundException : UserException
    {
        public UserNotFoundException() { }
        public UserNotFoundException(string message) : base(message) { }
        public UserNotFoundException(string message, Exception inner) : base(message, inner) { }

        public UserNotFoundException(string message, Exception inner, string userName) : base(message, inner) 
        {
            this.UserName = userName;
        }
        protected UserNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }


    }
}

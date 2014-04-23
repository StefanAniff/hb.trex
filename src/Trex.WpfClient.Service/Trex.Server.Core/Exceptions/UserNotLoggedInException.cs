using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Exceptions
{
    [global::System.Serializable]
    public class UserNotLoggedInException : ApplicationBaseException
    {
      
        public UserNotLoggedInException() { }
        public UserNotLoggedInException(string message) : base(message) { }
        public UserNotLoggedInException(string message, Exception inner) : base(message, inner) { }
        protected UserNotLoggedInException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

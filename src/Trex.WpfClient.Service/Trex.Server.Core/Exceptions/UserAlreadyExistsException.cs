﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Exceptions
{
    [global::System.Serializable]
    public class UserAlreadyExistsException : UserException
    {
        public UserAlreadyExistsException() { }
        public UserAlreadyExistsException(string message) : base(message) { }
        public UserAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        public UserAlreadyExistsException(string message, Exception inner, string userName) : base(message, inner) 
        {
            this.UserName = userName;
        }
        protected UserAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

    }
}

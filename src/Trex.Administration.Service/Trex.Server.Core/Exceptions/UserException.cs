﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Exceptions
{
    /// <summary>
    /// Base class for all user-related exceptions
    /// </summary>
    [global::System.Serializable]
    public class UserException : ApplicationBaseException
    {
        public UserException() { }
        public UserException(string message) : base(message) { }
        public UserException(string message, Exception inner) : base(message, inner) { }
        public UserException(string message, Exception inner, string userName) : base(message, inner) 
        {
            this.UserName = userName;
        }

        protected UserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// Gets or sets the username that caused the exception.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }
    }
}

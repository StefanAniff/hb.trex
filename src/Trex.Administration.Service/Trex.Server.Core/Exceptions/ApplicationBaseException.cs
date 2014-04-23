using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Exceptions
{
    /// <summary>
    /// Base exception for all exceptions in the application
    /// </summary>
    [global::System.Serializable]
    public class ApplicationBaseException : ApplicationException
    {
        public ApplicationBaseException() { }
        public ApplicationBaseException(string message) : base(message) { }
        public ApplicationBaseException(string message, Exception inner) : base(message, inner) { }
        protected ApplicationBaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

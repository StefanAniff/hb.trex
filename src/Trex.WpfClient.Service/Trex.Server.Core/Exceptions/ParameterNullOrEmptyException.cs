using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Exceptions
{
    [global::System.Serializable]
    public class ParameterNullOrEmptyException : ApplicationBaseException
    {
      
        public ParameterNullOrEmptyException() { }
        public ParameterNullOrEmptyException(string message) : base(message) { }
        public ParameterNullOrEmptyException(string message, Exception inner) : base(message, inner) { }
        public ParameterNullOrEmptyException(string message, Exception inner, string parameter) : base(message, inner) { this.Parameter = parameter; }

        protected ParameterNullOrEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public string Parameter { get; set; }
    }
}

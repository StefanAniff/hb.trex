using System;
using System.Runtime.Serialization;

namespace Trex.Server.Core.Exceptions
{
    [Serializable]
    public class ParameterNullOrEmptyException : ApplicationBaseException
    {
        public ParameterNullOrEmptyException() {}
        public ParameterNullOrEmptyException(string message) : base(message) {}
        public ParameterNullOrEmptyException(string message, Exception inner) : base(message, inner) {}

        public ParameterNullOrEmptyException(string message, Exception inner, string parameter) : base(message, inner)
        {
            Parameter = parameter;
        }

        protected ParameterNullOrEmptyException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {}

        public string Parameter { get; set; }
    }
}
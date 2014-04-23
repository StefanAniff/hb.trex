using System;

namespace Trex.Core.Exceptions
{
    public class ParameterNullOrEmptyException : ApplicationBaseException
    {
        public ParameterNullOrEmptyException() {}
        public ParameterNullOrEmptyException(string message) : base(message) {}
        public ParameterNullOrEmptyException(string message, Exception inner) : base(message, inner) {}

        public ParameterNullOrEmptyException(string message, Exception inner, string parameter) : base(message, inner)
        {
            Parameter = parameter;
        }

        public string Parameter { get; set; }
    }
}
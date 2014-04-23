using System;
using System.Runtime.Serialization;

namespace Trex.Server.Core.Exceptions
{
    /// <summary>
    /// Base exception for all exceptions in the application
    /// </summary>
    [Serializable]
    public class ApplicationBaseException : ApplicationException
    {
        public ApplicationBaseException() {}
        public ApplicationBaseException(string message) : base(message) {}
        public ApplicationBaseException(string message, Exception inner) : base(message, inner) {}

        protected ApplicationBaseException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {}
    }
}
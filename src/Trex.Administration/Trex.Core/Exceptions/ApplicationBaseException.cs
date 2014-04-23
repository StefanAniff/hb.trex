using System;

namespace Trex.Core.Exceptions
{
    /// <summary>
    /// Base exception for all exceptions in the application
    /// </summary>
    public class ApplicationBaseException : Exception
    {
        public ApplicationBaseException() {}
        public ApplicationBaseException(string message) : base(message) {}
        public ApplicationBaseException(string message, Exception inner) : base(message, inner) {}
    }
}
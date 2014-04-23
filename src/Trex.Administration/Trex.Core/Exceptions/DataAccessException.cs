using System;

namespace Trex.Core.Exceptions
{
    public class DataAccessException : ApplicationBaseException
    {
        public DataAccessException() {}
        public DataAccessException(string message) : base(message) {}
        public DataAccessException(string message, Exception inner) : base(message, inner) {}
    }
}
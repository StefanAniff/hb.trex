using System;
using System.Runtime.Serialization;

namespace Trex.Server.Core.Exceptions
{
    [Serializable]
    public class DataAccessException : ApplicationBaseException
    {
        public DataAccessException() {}
        public DataAccessException(string message) : base(message) {}
        public DataAccessException(string message, Exception inner) : base(message, inner) {}

        protected DataAccessException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {}
    }
}
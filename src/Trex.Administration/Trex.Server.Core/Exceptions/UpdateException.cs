using System;
using System.Runtime.Serialization;

namespace Trex.Server.Core.Exceptions
{
    [Serializable]
    public class EntityUpdateException : ApplicationBaseException
    {
        public EntityUpdateException() {}
        public EntityUpdateException(string message) : base(message) {}
        public EntityUpdateException(string message, Exception inner) : base(message, inner) {}

        protected EntityUpdateException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {}
    }
}
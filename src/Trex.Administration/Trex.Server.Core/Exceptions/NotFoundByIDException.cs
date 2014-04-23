using System;
using System.Runtime.Serialization;

namespace Trex.Server.Core.Exceptions
{
    [Serializable]
    public class NotFoundByIDException : ApplicationBaseException
    {
        public NotFoundByIDException() {}
        public NotFoundByIDException(string message) : base(message) {}
        public NotFoundByIDException(string message, Exception inner) : base(message, inner) {}

        protected NotFoundByIDException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {}

        public int Id { get; set; }
    }
}
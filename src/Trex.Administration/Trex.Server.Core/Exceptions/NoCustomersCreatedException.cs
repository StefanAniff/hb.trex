using System;
using System.Runtime.Serialization;

namespace Trex.Server.Core.Exceptions
{
    [Serializable]
    public class NoCustomersCreatedException : ApplicationBaseException
    {
        public NoCustomersCreatedException() {}
        public NoCustomersCreatedException(string message) : base(message) {}
        public NoCustomersCreatedException(string message, Exception inner) : base(message, inner) {}

        protected NoCustomersCreatedException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {}
    }
}
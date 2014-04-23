using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Trex.SmartClient.Core.Exceptions
{
    [Serializable]
    public class NotFoundByGuidException : AppBaseException
    {
        
        public NotFoundByGuidException()
        {
        }

        public NotFoundByGuidException(string message) : base(message)
        {
        }

        public NotFoundByGuidException(string message, Exception inner) : base(message, inner)
        {
        }

        public NotFoundByGuidException(string message, Guid guid, Exception inner) : base(message, inner)
        {
            Guid = guid;
        }

        public Guid Guid { get; private set; }


        protected NotFoundByGuidException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

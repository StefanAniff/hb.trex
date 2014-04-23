using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Exceptions
{
    [global::System.Serializable]
    public class EntityUpdateException : ApplicationBaseException
    {
        public EntityUpdateException() { }
        public EntityUpdateException(string message) : base(message) { }
        public EntityUpdateException(string message, Exception inner) : base(message, inner) { }
        protected EntityUpdateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

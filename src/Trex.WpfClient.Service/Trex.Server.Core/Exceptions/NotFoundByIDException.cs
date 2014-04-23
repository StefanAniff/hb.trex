using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Exceptions
{
    [global::System.Serializable]
    public class NotFoundByIDException : ApplicationBaseException
    {
        
        public NotFoundByIDException() { }
        public NotFoundByIDException(string message) : base(message) { }
        public NotFoundByIDException(string message, Exception inner) : base(message, inner) { }
        protected NotFoundByIDException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public int Id { get; set; }
    }
}

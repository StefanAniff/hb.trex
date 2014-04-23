using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Exceptions
{
    [global::System.Serializable]
    public class NoCustomersCreatedException : ApplicationBaseException
    {
     
        public NoCustomersCreatedException() { }
        public NoCustomersCreatedException(string message) : base(message) { }
        public NoCustomersCreatedException(string message, Exception inner) : base(message, inner) { }
        protected NoCustomersCreatedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

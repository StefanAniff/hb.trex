using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Trex.SmartClient.Core.Exceptions
{
    [Serializable]
    public class AppBaseException : ApplicationException
    {
       

        public AppBaseException()
        {
        }

        public AppBaseException(string message) : base(message)
        {
        }

        public AppBaseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AppBaseException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

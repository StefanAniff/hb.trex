using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Trex.Server.Core.Exceptions
{
    [Serializable]
    public class ActivationIdNotFoundException : Exception
    {

        public ActivationIdNotFoundException()
        {
        }

        public ActivationIdNotFoundException(string message) : base(message)
        {
        }

        public ActivationIdNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ActivationIdNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

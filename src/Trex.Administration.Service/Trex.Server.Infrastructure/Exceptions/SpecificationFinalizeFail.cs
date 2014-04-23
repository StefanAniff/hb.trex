using System;

namespace Trex.Server.Infrastructure.Exceptions
{
    [Serializable()]
    public class SpecificationFinalizeFail : System.Exception
    {
        public SpecificationFinalizeFail() : base() { }
        public SpecificationFinalizeFail(string message) : base(message) { }
        public SpecificationFinalizeFail(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected SpecificationFinalizeFail(System.Runtime.Serialization.SerializationInfo info,
                                             System.Runtime.Serialization.StreamingContext context) { }
    }
}

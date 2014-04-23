using System;

namespace Trex.Server.Infrastructure.Exceptions
{
    [Serializable()]
    public class InvoiceTemplateIdNotSet : System.Exception
    {
        public InvoiceTemplateIdNotSet() : base() { }
        public InvoiceTemplateIdNotSet(string message) : base(message) { }
        public InvoiceTemplateIdNotSet(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected InvoiceTemplateIdNotSet(System.Runtime.Serialization.SerializationInfo info,
                                             System.Runtime.Serialization.StreamingContext context) { }
    }
}
using System;

namespace Trex.Server.Infrastructure.Exceptions
{
    [Serializable()]
    public class InvoiceFinalizeFail : System.Exception
    {
        public InvoiceFinalizeFail() : base() { }
        public InvoiceFinalizeFail(string message) : base(message) { }
        public InvoiceFinalizeFail(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected InvoiceFinalizeFail(System.Runtime.Serialization.SerializationInfo info,
                                             System.Runtime.Serialization.StreamingContext context) { }
    }
}
using System;

namespace Trex.Server.Infrastructure.Exceptions
{
    [Serializable()]
    public class InvoiceFileNotFound : System.Exception
    {
        public InvoiceFileNotFound() : base() { }
        public InvoiceFileNotFound(string message) : base(message) { }
        public InvoiceFileNotFound(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected InvoiceFileNotFound(System.Runtime.Serialization.SerializationInfo info,
                                             System.Runtime.Serialization.StreamingContext context) { }
    }
}

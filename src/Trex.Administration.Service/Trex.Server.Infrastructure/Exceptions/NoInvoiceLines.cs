using System;

namespace Trex.Server.Infrastructure.Exceptions
{
    [Serializable()]
    public class NoInvoiceLines : System.Exception
    {
        public NoInvoiceLines() : base() { }
        public NoInvoiceLines(string message) : base(message) { }
        public NoInvoiceLines(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected NoInvoiceLines(System.Runtime.Serialization.SerializationInfo info,
                                 System.Runtime.Serialization.StreamingContext context) { }
    }
}
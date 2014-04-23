using System;

namespace Test_InvoiceBuilder
{
    [Serializable()]
    public class InvoicePrintMissing : System.Exception
    {
        public InvoicePrintMissing() : base() { }
        public InvoicePrintMissing(string message) : base(message) { }
        public InvoicePrintMissing(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected InvoicePrintMissing(System.Runtime.Serialization.SerializationInfo info,
                                      System.Runtime.Serialization.StreamingContext context) { }
    }
}
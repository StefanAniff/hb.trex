using System;

namespace Test_InvoiceBuilder
{
    [Serializable()]
    public class InvoiceMailMissing : System.Exception
    {
        public InvoiceMailMissing() : base() { }
        public InvoiceMailMissing(string message) : base(message) { }
        public InvoiceMailMissing(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected InvoiceMailMissing(System.Runtime.Serialization.SerializationInfo info,
                                     System.Runtime.Serialization.StreamingContext context) { }
    }
}
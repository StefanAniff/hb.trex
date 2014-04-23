using System;

namespace Test_InvoiceBuilder
{
    [Serializable()]
    public class CreditNotePrintMissing : System.Exception
    {
        public CreditNotePrintMissing() : base() { }
        public CreditNotePrintMissing(string message) : base(message) { }
        public CreditNotePrintMissing(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected CreditNotePrintMissing(System.Runtime.Serialization.SerializationInfo info,
                                         System.Runtime.Serialization.StreamingContext context) { }
    }
}
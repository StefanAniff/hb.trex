using System;

namespace Test_InvoiceBuilder
{
    [Serializable()]
    public class SpecificationPrintMissing : System.Exception
    {
        public SpecificationPrintMissing() : base() { }
        public SpecificationPrintMissing(string message) : base(message) { }
        public SpecificationPrintMissing(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected SpecificationPrintMissing(System.Runtime.Serialization.SerializationInfo info,
                                            System.Runtime.Serialization.StreamingContext context) { }
    }
}
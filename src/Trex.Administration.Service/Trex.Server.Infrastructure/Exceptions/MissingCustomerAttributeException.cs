using System;

namespace Test_InvoiceBuilder
{
    [Serializable()]
    public class MissingCustomerAttributeException : System.Exception
    {
        public MissingCustomerAttributeException() : base() { }
        public MissingCustomerAttributeException(string message) : base(message) { }
        public MissingCustomerAttributeException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected MissingCustomerAttributeException(System.Runtime.Serialization.SerializationInfo info,
                                             System.Runtime.Serialization.StreamingContext context) { }
    }
}
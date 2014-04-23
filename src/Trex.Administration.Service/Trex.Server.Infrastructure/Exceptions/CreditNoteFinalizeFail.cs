using System;

namespace Trex.Server.Infrastructure.Exceptions
{
    [Serializable()]
    public class CreditNoteFinalizeFail : System.Exception
    {
        public CreditNoteFinalizeFail() : base() { }
        public CreditNoteFinalizeFail(string message) : base(message) { }
        public CreditNoteFinalizeFail(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected CreditNoteFinalizeFail(System.Runtime.Serialization.SerializationInfo info,
                                         System.Runtime.Serialization.StreamingContext context) { }
    }
}
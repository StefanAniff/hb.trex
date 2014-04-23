using System;

namespace Trex.Server.Infrastructure.Exceptions
{
    [Serializable()]
    public class CreditNoteMailMissing : System.Exception
    {
        public CreditNoteMailMissing() : base() { }
        public CreditNoteMailMissing(string message) : base(message) { }
        public CreditNoteMailMissing(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected CreditNoteMailMissing(System.Runtime.Serialization.SerializationInfo info,
                                        System.Runtime.Serialization.StreamingContext context) { }
    }
}
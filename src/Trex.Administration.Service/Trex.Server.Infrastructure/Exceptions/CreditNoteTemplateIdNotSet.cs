using System;

namespace Trex.Server.Infrastructure.Exceptions
{
    [Serializable()]
    public class CreditNoteTemplateIdNotSet : System.Exception
    {
        public CreditNoteTemplateIdNotSet() : base() { }
        public CreditNoteTemplateIdNotSet(string message) : base(message) { }
        public CreditNoteTemplateIdNotSet(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected CreditNoteTemplateIdNotSet(System.Runtime.Serialization.SerializationInfo info,
                                             System.Runtime.Serialization.StreamingContext context) { }
    }
}
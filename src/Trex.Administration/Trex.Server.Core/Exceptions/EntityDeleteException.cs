using System;
using System.Runtime.Serialization;

namespace Trex.Server.Core.Exceptions
{
    [Serializable]
    public class EntityDeleteException : ApplicationBaseException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public EntityDeleteException() {}
        public EntityDeleteException(string message) : base(message) {}
        public EntityDeleteException(string message, Exception inner) : base(message, inner) {}

        protected EntityDeleteException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {}
    }
}
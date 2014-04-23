using System;
using System.Runtime.Serialization;

namespace TrexSL.Web.Exceptions
{
    [Serializable]
    public class RoleException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public RoleException()
        {
        }

        public RoleException(string message)
            : base(message)
        {
        }

        public RoleException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected RoleException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
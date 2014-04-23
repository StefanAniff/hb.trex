﻿using System;
using System.Runtime.Serialization;

namespace Trex.Server.Core.Exceptions
{
    [Serializable]
    public class VersionException : ApplicationBaseException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public VersionException() {}
        public VersionException(string message) : base(message) {}
        public VersionException(string message, Exception inner) : base(message, inner) {}

        protected VersionException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {}
    }
}
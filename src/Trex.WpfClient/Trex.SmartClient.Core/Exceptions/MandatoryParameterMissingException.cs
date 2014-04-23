﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Trex.SmartClient.Core.Exceptions
{
    [Serializable]
    public class MandatoryParameterMissingException : AppBaseException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MandatoryParameterMissingException()
        {
        }

        public MandatoryParameterMissingException(string message) : base(message)
        {
        }

        public MandatoryParameterMissingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MandatoryParameterMissingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Trex.SmartClient.Core.Exceptions
{
    [Serializable]
    public class FatalUnhandledException : AppBaseException
    {
        
        public FatalUnhandledException()
        {
        }

        public FatalUnhandledException(string message) : base(message)
        {
        }

        public FatalUnhandledException(string message, Exception inner) : base(message, inner)
        {
        }

        public FatalUnhandledException(string message, Exception inner,string userName)
            : base(message, inner)
        {
            UserName = userName;
            Date = DateTime.Now;
        }

        protected FatalUnhandledException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public string UserName { get; set; }

        public DateTime Date { get; set; }


    }
}

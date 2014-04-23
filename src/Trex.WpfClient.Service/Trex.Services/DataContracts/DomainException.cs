using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrexSL.Web.DataContracts
{
    public class DomainException : ApplicationException
    {
        public DomainException(string message, params object[] objs)
            : base(string.Format(message, objs))
        {

        }

        public DomainException(Exception innerException, string message, params object[] objs)
            : base(string.Format(message, objs), innerException)
        {

        }
    }
}
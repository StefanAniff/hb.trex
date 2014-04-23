using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Trex.SmartClient.Core.Exceptions
{
    [Serializable]
    public class MissingHieracleDataException : AppBaseException
    {
        public MissingHieracleDataException()
        {
        }

        public MissingHieracleDataException(string message)
            : base(message)
        {
        }

        public MissingHieracleDataException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected MissingHieracleDataException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class ResponseHeaders
    {
        public const string Compression = "compression";
        public const string ResponseType = "ResponseType";

        [DataContract]
        public class CompressionModes
        {
            public const string GZip = "gzip";
        }

    }
}
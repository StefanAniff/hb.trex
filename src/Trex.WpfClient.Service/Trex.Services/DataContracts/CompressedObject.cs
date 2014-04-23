using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class CompressedObject
    {
        public CompressedObject()
        {
            ResponseHeaders = new Dictionary<string, string>();
        }

        [DataMember]
        public Dictionary<string, string> ResponseHeaders { get; set; }

        [DataMember]
        public byte[] SerializedResponse { get; set; }
    }
}
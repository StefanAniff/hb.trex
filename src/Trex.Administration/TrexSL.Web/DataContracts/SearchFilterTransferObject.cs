using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class SearchFilterTransferObject
    {
        [DataMember]
        public List<int> Customers { get; set; }

        [DataMember]
        public List<int> Projects { get; set; }

        [DataMember]
        public List<int> Tasks { get; set; }

        [DataMember]
        public List<int> Users { get; set; }

        [DataMember]
        public DateTime? DateFrom { get; set; }

        [DataMember]
        public DateTime? DateTo { get; set; }
    }
}
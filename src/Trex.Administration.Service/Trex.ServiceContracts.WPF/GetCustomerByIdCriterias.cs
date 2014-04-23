using System;
using System.Runtime.Serialization;

namespace Trex.ServiceContracts
{
    [DataContract]
    public class GetCustomerByIdCriterias
    {
        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public DateTime TimeEntryFrom { get; set; }

        [DataMember]
        public DateTime TimeEntryTo { get; set; }

        [DataMember]
        public DateTime TaskFrom { get; set; }

        [DataMember]
        public DateTime TaskTo { get; set; }

        [DataMember]
        public bool IncludeInactive { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Trex.Common.DataTransferObjects
{
    [DataContract]
    public class TimeEntryUpdatedStatusDto
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public bool IsOK { get; set; }

        [DataMember]
        public string ReasonText { get; set; }
    }
}

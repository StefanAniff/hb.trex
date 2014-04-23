using System;
using System.Runtime.Serialization;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class TimeEntryType
    {
        [DataMember]
        public virtual int Id { get; set; }

        [DataMember]
        public virtual Guid Guid { get; set; }

        [DataMember]
        public virtual bool IsDefault { get; set; }

        [DataMember]
        public virtual bool IsBillableByDefault { get; set; }

        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual int? CustomerId { get; set; }
    }
}
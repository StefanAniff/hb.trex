using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class Project
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Company Company { get; set; }

        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public User CreatedBy { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public List<Task> Tasks { get; set; }

        [DataMember]
        public bool Inactive { get; set; }

        [DataMember]
        public bool IsEstimatesEnabled { get; set; }

        [DataMember]
        public DateTime? ChangeDate { get; set; }

        [DataMember]
        public User ChangedBy { get; set; }
    }
}
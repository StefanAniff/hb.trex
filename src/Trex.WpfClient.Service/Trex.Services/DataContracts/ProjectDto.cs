using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class ProjectDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int CompanyId { get; set; }
        
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string CreatedByName { get; set; }
        
        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public bool Inactive { get; set; }

        [DataMember]
        public bool IsEstimatesEnabled { get; set; }

        [DataMember]
        public DateTime? ChangeDate { get; set; }

        [DataMember]
        public string ChangedByName { get; set; }
    }
}
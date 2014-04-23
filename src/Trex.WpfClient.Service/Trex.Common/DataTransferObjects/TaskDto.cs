using System;
using System.Runtime.Serialization;

namespace Trex.Common.DataTransferObjects
{
    [DataContract]
    public class TaskDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? ParentTaskId { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string CreatedByName { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public double TimeLeft { get; set; }

        [DataMember]
        public double TimeEstimated { get; set; }

        [DataMember]
        public double WorstCaseEstimate { get; set; }

        [DataMember]
        public double BestCaseEstimate { get; set; }

        [DataMember]
        public double RealisticEstimate { get; set; }

        [DataMember]
        public double RegisteredTime { get; set; }

        [DataMember]
        public int ProjectId { get; set; }

        [DataMember]
        public DateTime? ChangeDate { get; set; }

        [DataMember]
        public string ChangedByName { get; set; }

        [DataMember]
        public TimeRegistrationTypeEnum TimeRegistrationType { get; set; }

        [DataMember]
        public bool Inactive { get; set; }
    }
}
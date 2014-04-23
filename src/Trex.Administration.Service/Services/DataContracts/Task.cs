using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class Task
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Task ParentTask { get; set; }

        [DataMember]
        public int? ParentTaskId { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public User CreatedBy { get; set; }

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
        public bool Closed { get; set; }

        [DataMember]
        public Project Project { get; set; }

        [DataMember]
        public int ProjectId { get; set; }

        [DataMember]
        public List<TimeEntry> TimeEntries { get; set; }

        [DataMember]
        public List<Task> SubTasks { get; set; }

        [DataMember]
        public DateTime? ChangeDate { get; set; }

        [DataMember]
        public User ChangedBy { get; set; }
    }
}
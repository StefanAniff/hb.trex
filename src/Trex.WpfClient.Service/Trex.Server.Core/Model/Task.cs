using System;
using System.Collections.Generic;

namespace Trex.Server.Core.Model
{
    public class Task : EntityBase
    {
        public Task()
        {
            TimeEntries = new List<TimeEntry>();
            Guid = Guid.NewGuid();
        }

        public virtual int TaskID { get; set; }

        public virtual Task ParentTask { get; set; }

        public virtual Guid Guid { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual DateTime CreateDate { get; set; }

        public virtual string TaskName { get; set; }

        public virtual string Description { get; set; }

        public virtual double TimeLeft { get; set; }

        public virtual double TimeEstimated { get; set; }

        public virtual double WorstCaseEstimate { get; set; }

        public virtual double BestCaseEstimate { get; set; }

        public virtual double RealisticEstimate { get; set; }

        public virtual bool Inactive { get; set; }

        public virtual Project Project { get; set; }

        public virtual DateTime? ChangeDate { get; set; }

        public virtual User ChangedBy { get; set; }

        public virtual Tag Tag { get; set; }

        public virtual IList<TimeEntry> TimeEntries { get; set; }

        public virtual int TimeRegistrationTypeId { get; set; }

        public virtual TimeRegistrationTypeEnum TimeRegistrationTypeEnum
        {
            get { return (TimeRegistrationTypeEnum) TimeRegistrationTypeId; }
        }

        public override int EntityId
        {
            get { return TaskID; }
        }
    }

    public enum TimeRegistrationTypeEnum : int
    {
        Standard = 0,
        Projection = 1,
        Vacation = 2,
    }
}
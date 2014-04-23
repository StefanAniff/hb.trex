using System;
using System.Collections.Generic;
using System.Linq;
using Trex.Server.Core.Exceptions;

namespace Trex.Server.Core.Model
{
    /// <summary>
    /// Task object for NHibernate mapped table 'Tasks'.
    /// </summary>
    public class Task
    {
        #region Constructors

        public Task()
        {
            TimeEntries = new List<TimeEntry>();
            SubTasks = new List<Task>();
            Guid = Guid.NewGuid();
        }

        public Task(string taskName, string description, User createdBy, DateTime createDate, DateTime changeDate, double timeLeft, double timeEstimated, double bestCaseEstimate, double worstCaseEstimate,
                    double realisticEstimate, bool closed, Project project, Tag tag, Task parentTask)
            : this()
        {
            Name = taskName;
            Description = description;
            CreatedBy = createdBy;
            CreateDate = createDate;
            ChangeDate = changeDate;
            TimeLeft = timeLeft;
            TimeEstimated = timeEstimated;
            WorstCaseEstimate = worstCaseEstimate;
            BestCaseEstimate = bestCaseEstimate;
            RealisticEstimate = realisticEstimate;
            Closed = closed;
            Project = project;
            Tag = tag;
            ParentTask = parentTask;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent task.
        /// </summary>
        /// <value>The parent task.</value>
        public virtual Task ParentTask { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public virtual Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public virtual User CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        /// <value>The name of the task.</value>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets the time used on a task, excluding pause time
        /// </summary>
        /// <value>The time used.</value>
        public virtual double TotalTimeUsed
        {
            get
            {
                double timeUsed = 0;

                foreach (var entry in TimeEntries)
                {
                    timeUsed += entry.TimeSpent;
                }

                return timeUsed;
            }
        }

        public virtual double BillableTime
        {
            get
            {
                double billableTimeUsed = 0;
                foreach (var entry in TimeEntries)
                {
                    billableTimeUsed += entry.BillableTime;
                }

                return billableTimeUsed;
            }
        }

        /// <summary>
        /// Gets or sets the time left.
        /// </summary>
        /// <value>The time left.</value>
        public virtual double TimeLeft { get; set; }

        /// <summary>
        /// Gets or sets the time estimated. This is a calculated value from the 3-point estimate
        /// </summary>
        /// <value>The time estimated.</value>
        public virtual double TimeEstimated { get; set; }

        /// <summary>
        /// Gets or sets the worst case estimate.
        /// </summary>
        /// <value>The worst case estimate.</value>
        public virtual double WorstCaseEstimate { get; set; }

        /// <summary>
        /// Gets or sets the best case estimate.
        /// </summary>
        /// <value>The best case estimate.</value>
        public virtual double BestCaseEstimate { get; set; }

        /// <summary>
        /// Gets or sets the realistic estimate.
        /// </summary>
        /// <value>The realistic estimate.</value>
        public virtual double RealisticEstimate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Task"/> is closed.
        /// </summary>
        /// <value><c>true</c> if closed; otherwise, <c>false</c>.</value>
        public virtual bool Closed { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        public virtual Project Project { get; set; }

        public virtual DateTime? ChangeDate { get; set; }

        public virtual User ChangedBy { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public virtual Tag Tag { get; set; }

        /// <summary>
        /// Gets or sets the time regs.
        /// </summary>
        /// <value>The time regs.</value>
        public virtual IList<TimeEntry> TimeEntries { get; set; }

        /// <summary>
        /// Gets or sets the sub tasks.
        /// </summary>
        /// <value>The sub tasks.</value>
        public virtual IList<Task> SubTasks { get; set; }

        #endregion

        /// <summary>
        /// Gets the total time including sub tasks
        /// </summary>
        /// <returns></returns>
        public virtual double GetTotalTimeIncludingSubTasks()
        {
            var timeUsed = TotalTimeUsed;
            foreach (var task in SubTasks)
            {
                timeUsed += task.TotalTimeUsed;
            }

            return timeUsed;
        }

        /// <summary>
        /// Gets the total billable time including sub tasks.
        /// </summary>
        /// <returns></returns>
        public virtual double GetTotalBillableTimeIncludingSubTasks()
        {
            var time = BillableTime;

            foreach (var task in SubTasks)
            {
                time += task.BillableTime;
            }

            return time;
        }

        public virtual void RemoveTimeEntry(int timeEntryId)
        {
            var timeEntry = TimeEntries.Single(te => te.Id == timeEntryId);

            if (timeEntry == null)
            {
                throw new EntityDeleteException("Timeentry not found in task: " + timeEntryId);
            }

            if (timeEntry.Invoice != null)
            {
                throw new EntityDeleteException("Timeentry cannot be deleted when billed: Id:" + timeEntry.Id);
            }

            TimeEntries.Remove(timeEntry);
        }
    }
}
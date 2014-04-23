

using System;
using Trex.ServiceContracts.Model;

namespace Trex.ServiceContracts
{
    public partial class TimeEntry : IEntity
    {
        public TimeEntry()
        {
            Guid = Guid.NewGuid();
        }

        public bool IsValidChild(IEntity entity)
        {
            return false;
        }

        public int Id
        {
            get { return TimeEntryID; }
            set { TimeEntryID = value; }
        }

        
        public bool IsStopped { get; set; }

        public ServiceClients ServiceClient
        {
            get { return (ServiceClients)ClientSourceId; }
            set { ClientSourceId = (int) value; }
        }

        public override bool Equals(object obj)
        {
            var timeEntry = obj as TimeEntry;

            if (timeEntry == null)
                return false;

            if (TimeEntryID == 0)
                return Guid == timeEntry.Guid;

            return timeEntry.TimeEntryID == this.TimeEntryID;

        }

        public override int GetHashCode()
        {
            return TimeEntryID.GetHashCode();
        }

        public bool IsSynced { get; set; }

        /// <summary>
        /// Creates a timeentry with default values
        /// </summary>
        /// <returns></returns>
        public static TimeEntry Create()
        {
            return new TimeEntry
            {
                Guid = Guid.NewGuid(),
                BillableTime = 0d,
                TimeSpent = 0d,
                Description = string.Empty,
                Task = null,
                Price = 0d,
                //CreatedDate = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                IsSynced = false,
                Billable = true,
                
            };
        }


        public void CancelChanges()
        {
            ChangeTracker.SetParentObject(this);
            ChangeTracker.CancelChanges();
        }



        /// <summary>
        /// Creates a new timeentry based on a given timeentry (with timespent Zero)
        /// </summary>
        /// <param name="timeEntry">The time entry.</param>
        /// <returns></returns>
        public static TimeEntry Create(TimeEntry timeEntry)
        {

            if (timeEntry == null)
                return Create();
            return new TimeEntry
            {
                Guid = Guid.NewGuid(),
                BillableTime = 0d,
                TimeSpent = 0d,
                Description = string.Empty,
                Task = timeEntry.Task,
                Price = timeEntry.Price,
                //CreatedDate = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                IsSynced = false,
               
                Billable = timeEntry.Billable,
                
            };
        }

        /// <summary>
        /// Creates a Timeentry, assigned to the given task
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        /// <exception cref="MandatoryParameterMissingException"></exception>
        public static TimeEntry Create(Task task, TimeEntryType timeEntryType)
        {
          

            return Create(
                System.Guid.NewGuid(),
                task,
                timeEntryType,
                0d,
               0d,
                string.Empty,
                DateTime.Now,
                DateTime.Now,
                0d,
                false,
                string.Empty,
                true,
                
                false);

        }

        /// <summary>
        /// Creates the specified timeentry, by the given parameters.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="task">The task.</param>
        /// <param name="timeSpent">The time spent.</param>
        /// <param name="billableTime">The billable time.</param>
        /// <param name="description">The description.</param>
        /// <param name="createDate">The create date.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="pricePrHour">The price pr hour.</param>
        /// <param name="isSynced">if set to <c>true</c> [is synced].</param>
        /// <param name="syncResponse">The sync response.</param>
        /// <param name="billable">if set to <c>true</c> [billable].</param>
        /// <returns></returns>
        /// <exception cref="MandatoryParameterMissingException"></exception>
        public static TimeEntry Create(Guid guid, Task task, TimeEntryType timeEntryType, double timeSpent, double billableTime, string description, DateTime startTime, DateTime endTime, double pricePrHour, bool isSynced, string syncResponse, bool billable,  bool isStopped)
        {
            //if (task == null)
            //    throw new MandatoryParameterMissingException("Error creating Timeentry: Task cannot be null");

            return new TimeEntry
            {
                Guid = guid,
                TimeEntryType = timeEntryType,
                BillableTime = billableTime,
                TimeSpent = timeSpent,
                Description = description,
                Task = task,
                IsStopped = isStopped,

                Price = 0d,
                //CreatedDate = createDate,
                StartTime = startTime,
                EndTime = endTime,
                IsSynced = isSynced,
               
                Billable = billable,



            };
        }

    }
}

using System.Linq;
using Trex.Core.Exceptions;
using Trex.ServiceContracts;

namespace Trex.Core.Extensions
{
    public static class ModelExtensions
    {
        public static double GetTotalBillableTimeSpent(this Project project)
        {
            return project.Tasks.Sum(task => task.BillableTime());
        }


        /// <summary>
        /// Gets the time used on a task, excluding pause time
        /// </summary>
        /// <value>The time used.</value>
        public static double TotalTimeUsed(this Task task)
        {
            return task.TimeEntries.Sum(entry => entry.TimeSpent);
        }


        public static double BillableTime(this Task task)
        {
            return task.TimeEntries.Sum(entry => entry.BillableTime);
        }

        ///// <summary>
        ///// Gets the total time including sub tasks
        ///// </summary>
        ///// <returns></returns>
        //public static double GetTotalTimeIncludingSubTasks(this Task task)
        //{
        //    var timeUsed = TotalTimeUsed;
        //    foreach (var task in task.SubTasks)
        //    {
        //        timeUsed += task.TotalTimeUsed;
        //    }

        //    return timeUsed;
        //}

        ///// <summary>
        ///// Gets the total billable time including sub tasks.
        ///// </summary>
        ///// <returns></returns>
        //public virtual double GetTotalBillableTimeIncludingSubTasks()
        //{
        //    var time = BillableTime;

        //    foreach (var task in SubTasks)
        //    {
        //        time += task.BillableTime;
        //    }

        //    return time;
        //}

        public static void RemoveTimeEntry(this Task task, int timeEntryId)
        {
            var timeEntry = task.TimeEntries.Single(te => te.TimeEntryID == timeEntryId);

            if (timeEntry == null)
            {
                throw new EntityDeleteException("Timeentry not found in task: " + timeEntryId);
            }

            if (!timeEntry.InvoiceId.HasValue)
            {
                throw new EntityDeleteException("Timeentry cannot be deleted when billed: Id:" + timeEntry.TimeEntryID);
            }

            task.TimeEntries.Remove(timeEntry);
        }
    }
}

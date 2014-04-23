using System;

namespace Trex.Core.Model
{
    public class TaskManagementFilters
    {
        public TaskManagementFilters()
        {
            var dateNow = DateTime.Now.Date;

            // Task default last 2 months
            TaskFilterFrom = dateNow.AddMonths(-2);
            TaskFilterTo = dateNow;

            // TimeEntry default last 2 months
            TimeEntryFilterFrom = dateNow.AddMonths(-2);
            TimeEntryFilterTo = dateNow;
        }

        public bool ShowInactive { get; set; }
        public DateTime TimeEntryFilterFrom { get; set; }         
        public DateTime TimeEntryFilterTo { get; set; }

        public DateTime TaskFilterFrom { get; set; }
        public DateTime TaskFilterTo { get; set; }
    }
}
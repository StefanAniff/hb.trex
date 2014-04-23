using System.Collections.Generic;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class TimeEntriesListEventArgs : System.EventArgs
    {
        public TimeEntriesListEventArgs(List<TimeEntry> timeEntries)
        {
            if (timeEntries != null)
            {
                TimeEntries = timeEntries;
            }
            else
            {
                TimeEntries = new List<TimeEntry>();
            }
        }

        public List<TimeEntry> TimeEntries { get; set; }
    }
}
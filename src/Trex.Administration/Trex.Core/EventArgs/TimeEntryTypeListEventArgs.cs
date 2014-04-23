using System.Collections.Generic;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class TimeEntryTypeListEventArgs : System.EventArgs
    {
        public TimeEntryTypeListEventArgs(List<TimeEntryType> timeEntryTypes)
        {
            TimeEntryTypes = timeEntryTypes;
        }

        public List<TimeEntryType> TimeEntryTypes { get; set; }
    }
}
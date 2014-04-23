using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class TimeEntrySaveEventArgs : System.EventArgs
    {
        public TimeEntrySaveEventArgs(TimeEntry timeEntry)
        {
            TimeEntry = timeEntry;
        }

        public TimeEntry TimeEntry { get; set; }
    }
}
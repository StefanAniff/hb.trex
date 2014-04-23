using Trex.ServiceModel.Model;

namespace Trex.Core.EventArgs
{
    public class TimeEntryTypeSaveEventArgs : System.EventArgs
    {
        public TimeEntryTypeSaveEventArgs(TimeEntryType timeEntryType)
        {
            TimeEntryType = timeEntryType;
        }

        public TimeEntryType TimeEntryType { get; set; }
    }
}
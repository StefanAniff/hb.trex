using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Core.Model
{
    public class TimeEntryHistory
    {
        public TimeEntryHistory()
        {
            TimeLog = new List<TimeInterval>();
        }

        public List<TimeInterval> TimeLog { get; set; }

        public TimeSpan TotalTime()
        {
            TimeSpan timeSpent = TimeSpan.Zero;

            
            foreach (var timeInterval in TimeLog)
            {
                timeSpent = timeSpent.Add(timeInterval.Interval);
            }
            return timeSpent;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Core.Model
{
    public class TimeInterval
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Interval { get { return EndTime - StartTime; } }
    }
}

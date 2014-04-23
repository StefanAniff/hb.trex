using System.Collections.Generic;

namespace Trex.SmartClient.Core.Model
{
    public class UserStatistics
    {
        public double RegisteredHoursToday { get; set; }
        public double RegisteredHoursThisWeek { get; set; }
        public double RegisteredHoursThisMonth { get; set; }
        public double BillableHoursToday { get; set; }
        public double BillableHoursThisWeek { get; set; }
        public double BillableHoursThisMonth { get; set; }
        public double EarningsToday { get; set; }
        public double EarningsThisWeek { get; set; }
        public double EarningsThisMonth { get; set; }
        public List<TimeEntry> LastXDaysTimeEntries { get; set; }

        public UserStatistics()
        {
            LastXDaysTimeEntries = new List<TimeEntry>();
        }
    }
}
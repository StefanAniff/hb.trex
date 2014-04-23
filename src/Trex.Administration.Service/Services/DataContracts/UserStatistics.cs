using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class UserStatistics
    {
        [DataMember]
        public double RegisteredHoursToday { get; set; }
        [DataMember]
        public double RegisteredHoursThisWeek { get; set; }
        [DataMember]
        public double RegisteredHoursThisMonth { get; set; }
        [DataMember]
        public double EarningsToday { get; set; }
        [DataMember]
        public double EarningsThisWeek { get; set; }
        [DataMember]
        public double EarningsThisMonth { get; set; }
        [DataMember]
        public double BillableHoursToday { get; set; }
        [DataMember]
        public double BillableHoursThisWeek { get; set; }
        [DataMember]
        public double BillableHoursThisMonth { get; set; }

        [DataMember]
        public List<TimeEntry> ThisWeeksTimeEntries { get; set; }
    }
}
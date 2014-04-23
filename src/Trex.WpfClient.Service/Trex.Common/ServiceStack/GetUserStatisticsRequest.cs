using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ServiceStack.ServiceHost;

namespace Trex.Common.ServiceStack
{
    [DataContract]
    public class GetUserStatisticsRequest : ReadonlyRequest, IReturn<GetUserStatisticsResponse>
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int NumberOfDaysBack { get; set; }
    }

    [DataContract]
    public class GetUserStatisticsResponse
    {
        [DataMember]
        public UserStatistics UserStatistics { get; set; }

    }

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
    }
}
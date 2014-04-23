using System;
using ServiceStack.ServiceHost;
using Trex.Common.DataTransferObjects;

namespace Trex.Common.ServiceStack
{
    public class ForecastStatisticsRequest : IReturn<ForecastStatisticsResponse>
    {
        public int UserId { get; set; }
        public int DisplayedMonth { get; set; }
        public int DisplayedYear { get; set; }
        public DateTime Now { get; set; }
        public bool WorkPlanRealizedHourBillableOnly { get; set; }
    }

    public class ForecastStatisticsResponse
    {
        public ForecastStatisticsDto ForecastStatistics { get; set; }
    }
}
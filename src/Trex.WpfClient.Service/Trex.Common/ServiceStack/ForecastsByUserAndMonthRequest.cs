using System.Collections.Generic;
using ServiceStack.ServiceHost;
using Trex.Common.DataTransferObjects;

namespace Trex.Common.ServiceStack
{
    public class ForecastsByUserAndMonthRequest : IReturn<ForecastsByUserAndMonthResponse>
    {
        public int UserId { get; set; }

        public int ForecastMonth { get; set; }
        public int ForecastYear { get; set; }

        public int HolidayMonth { get; set; }
        public int HolidayYear { get; set; }
    }

    public class ForecastsByUserAndMonthResponse
    {
        public ForecastMonthDto ForecastMonth { get; set; }
        public List<HolidayDto> Holidays { get; set; }
        public int ProjectForecastTypeId { get; set; } 
    }    
}
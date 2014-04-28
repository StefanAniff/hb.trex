using System.Collections.Generic;
using ServiceStack.ServiceHost;
using Trex.Common.DataTransferObjects;

namespace Trex.Common.ServiceStack
{
    public class ForecastSearchByRegistrationRequest : IReturn<ForecastSearchResponse>
    {
        public int ForecastMonth { get; set; }
        public int ForecastYear { get; set; }

        public List<int> UserIds { get; set; }

        // Optional. Though one must be filled
        public int? CompanyId { get; set; }
        public int? ProjectId { get; set; }

        public int? ForecastTypeId { get; set; }
    }

    public class ForecastSearchByUsersRequest : IReturn<ForecastSearchResponse>
    {
        public int ForecastMonth { get; set; }
        public int ForecastYear { get; set; }
        public List<int> UserIds { get; set; }
        public bool GetAllUsers { get; set; }
    }

    public class ForecastSearchResponse
    {
        public List<ForecastMonthDto> ForecastMonths { get; set; }
        public List<HolidayDto> Holidays { get; set; }
        public int ProjectForecastTypeId { get; set; } 
    }
}
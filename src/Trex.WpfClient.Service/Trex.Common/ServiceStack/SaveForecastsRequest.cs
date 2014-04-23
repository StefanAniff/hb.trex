using ServiceStack.ServiceHost;
using Trex.Common.DataTransferObjects;

namespace Trex.Common.ServiceStack
{
    public class SaveForecastsRequest : IReturn<SaveForecastsResponse>
    {
        public ForecastMonthDto ForecastMonthDto { get; set; }
    }

    public class SaveForecastsResponse
    {
        /// <summary>
        /// When a new forecastmonth is saved. 
        /// Return the new id.
        /// </summary>
        public int ForecastMonthId { get; set; }
    }
}
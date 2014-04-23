using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;

namespace Trex.SmartClient.Core.Services
{
    public interface IForecastService
    {
        Task<ForecastsByUserAndMonthResponse> GetByUserIdAndMonth(int userId, int month, int year);
        Task<ForecastTypesResponse> GetForecastTypes();
        Task<SaveForecastsResponse> SaveForecasts(ForecastMonthDto forecastMonth);
        Task<ForecastStatisticsResponse> GetForecastStatistics(DateTime currentMonth, int userId);
        Task<ForecastsByUserAndMonthResponse> GetByUserIdAndMonth(int userId, int forecastMonth, int forecastYear, int holidayMonth, int holidayYear);
        Task<ForecastSearchResponse> GetBySearch(int forecastMonth, int forecastYear, int? projectId, int? companyId, int? forecastTypeId);
        Task<ForecastSearchOptionsResponse> GetOverivewSearchOptions();
        Task<ForecastSearchResponse> GetBySearch(int forecastMonth, int forecastYear, IEnumerable<int> userIds);
    }
}
using System;
using System.Collections.Generic;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.SmartClient.Core.Services;
using System.Threading.Tasks;
using System.Linq;

namespace Trex.SmartClient.Service
{
    public class ForecastService : ClientServiceBase, IForecastService
    {
        private readonly IAppSettings _appSettings;

        public ForecastService(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<SaveForecastsResponse> SaveForecasts(ForecastMonthDto forecastMonth)
        {
            var request = new SaveForecastsRequest
                {
                    ForecastMonthDto = forecastMonth
                };

            var result = await TrySendAsync(request, () => "An error occured while saving forecastMonth");
            return result;
        }

        /// <summary>
        /// Gets Forecasts and Holidays in same month and year
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<ForecastsByUserAndMonthResponse> GetByUserIdAndMonth(int userId, int month, int year)
        {
            return await GetByUserIdAndMonth(userId, month, year, month, year);
        }

        /// <summary>
        /// Gets Forecasts and Holidays in given month and year respectively
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="forecastMonth"></param>
        /// <param name="forecastYear"></param>
        /// <param name="holidayMonth"></param>
        /// <param name="holidayYear"></param>
        /// <returns></returns>
        public async Task<ForecastsByUserAndMonthResponse> GetByUserIdAndMonth(int userId, int forecastMonth, int forecastYear, int holidayMonth, int holidayYear)
        {
            var request = new ForecastsByUserAndMonthRequest
            {
                UserId = userId,
                ForecastMonth = forecastMonth,
                ForecastYear = forecastYear,
                HolidayMonth = holidayMonth,
                HolidayYear = holidayYear
            };

            var result = await TrySendAsync(request, () => string.Format("An error occured while getting forecastMonth for month {0} year {1}", forecastMonth, forecastYear));
            return result;
        }

        public async Task<ForecastTypesResponse> GetForecastTypes()
        {
            var result = await TrySendAsync(new ForecastTypesRequest(), () => "An error occured while getting forecast-types");
            return result;
        }

        public async Task<ForecastStatisticsResponse> GetForecastStatistics(DateTime currentMonth, int userId)
        {
            var result = await TrySendAsync(new ForecastStatisticsRequest
                {
                    UserId = userId,
                    DisplayedMonth = currentMonth.Month,
                    DisplayedYear = currentMonth.Year,
                    Now = DateTime.Now,
                    WorkPlanRealizedHourBillableOnly = _appSettings.WorkPlanRealizedHourBillableOnly
                }
                , () => "An error occured while getting forecast-statistics");
            return result;
        }

        public async Task<ForecastSearchResponse> GetBySearch(int forecastMonth, int forecastYear, int? projectId, int? companyId, int? forecastTypeId)
        {
            var result = await TrySendAsync(new ForecastSearchByRegistrationRequest
                {
                    ForecastMonth = forecastMonth,
                    ForecastYear = forecastYear,
                    ProjectId = projectId,
                    CompanyId = companyId,
                    ForecastTypeId = forecastTypeId,
                }
                , () => "An error occured while searching for forecasts");

            return result;
        }

        public async Task<ForecastSearchResponse> GetBySearch(int forecastMonth, int forecastYear, IEnumerable<int> userIds)
        {
            var result = await TrySendAsync(new ForecastSearchByUsersRequest
                {
                    ForecastMonth = forecastMonth,
                    ForecastYear = forecastYear,
                    UserIds = userIds.ToList()
                }
                , () => "An error occured while searching for forecasts");

            return result;
        }        

        public async Task<ForecastSearchOptionsResponse> GetOverivewSearchOptions()
        {
            return await TrySendAsync(new ForecastSearchOptionsRequest(), () => "An error occured while getting search optionns");
        }
    }
}
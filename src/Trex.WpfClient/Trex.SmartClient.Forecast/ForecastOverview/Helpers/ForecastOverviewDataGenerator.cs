using System;
using System.Collections.Generic;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Core.Extensions;
using System.Linq;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastOverview.Helpers
{
    public class ForecastOverviewDataGenerator
    {

        public ForecastDates CreateDatesFromDate(DateTime dateTime)
        {
            return new ForecastDates(dateTime
                                    .CreateDatesForMonth()
                                    .OrderBy(x => x.Day)
                                    .Select(x => new ForecastDate(x)));
        }

        /// <summary>
        /// Sets date-relating DateColumns to Holiday
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="holidays"></param>
        public virtual void MergeHolidays(IList<ForecastDate> targets, List<HolidayDto> holidays)
        {
            foreach (var holiday in holidays)
            {
                // Must exist. If not there is something wrong with date-generation
                var toUpdate = targets.Single(x => x.Date == holiday.Date);
                toUpdate.SetToHoliday(holiday);
            }
        }

        /// <summary>
        /// Updates 
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="source"></param>
        /// <param name="projectForecastTypeId"></param>
        public void CreateForecastMonths(IForecastOverviewViewModel vm, IList<ForecastMonthDto> source, int projectForecastTypeId)
        {
            foreach (var forecastMonthDto in source)
            {
                var usrMonth = new ForecastOverviewForecastMonth
                    {
                        UserId = forecastMonthDto.UserId,
                        UserName = forecastMonthDto.UserName,
                        Forecasts = new List<ForecastOverviewForecast>(forecastMonthDto.ForecastDtos != null 
                                                                        ? forecastMonthDto.ForecastDtos.Select(x => x.ToClient())
                                                                        : new List<ForecastOverviewForecast>())
                    };

                // Add missing non-work (weekend/vaccation) days (not stored on server)
                foreach (var forecastDate in vm.Dates.Where(x => !usrMonth.Forecasts.Any(y => y.Date.Equals(x))))
                {
                    usrMonth.Forecasts.Add(new ForecastOverviewForecast { Date = forecastDate });
                }

                usrMonth.Forecasts = usrMonth.Forecasts.OrderBy(x => x.Date.Date).ToList();
                vm.UserRegistrations.Add(usrMonth);
            }


        }
    }
}
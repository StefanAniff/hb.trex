using System;
using System.Collections.Generic;
using AutoMapper;
using Trex.Common.DataTransferObjects;
using System.Linq;

namespace Trex.SmartClient.Forecast.ForecastRegistration.Helpers
{
    public class MostFrequentDayLayoutSelector
    {
        static MostFrequentDayLayoutSelector()
        {
            Mapper.CreateMap<ForecastDto, ForecastDtoWrapper>();
            Mapper.AssertConfigurationIsValid();
        }

        public virtual ForecastDto MostFrequentDayLayout(DayOfWeek dayOfWeek, IEnumerable<ForecastDto> dtos)
        {
            var wrappedByDay = dtos
                .Where(y => y.Date.DayOfWeek == dayOfWeek)
                .Select(ForecastDtoWrapper.CreateFrom).ToList();

            // Can be null, of previous month copied from is empty
            if (wrappedByDay.Count == 0)
                return null;

            // Middle stage for easier debugging
            var grouped = wrappedByDay.GroupBy(x => new
                    {
                        x.ForecastType.Id,
                        x.DedicatedForecastTypeHours,
                        x.ProjectHoursToString
                    })
                .OrderByDescending(grp => grp.Count())
                .ToList();

            return grouped
                .First()
                .FirstOrDefault();            
        }
        
        /// <summary>
        /// Wrapper mainly used for grouping, since grouping requires a property to evaluate on.
        /// </summary>
        public class ForecastDtoWrapper : ForecastDto
        {
            public static ForecastDtoWrapper CreateFrom(ForecastDto dto)
            {
                return Mapper.Map<ForecastDto, ForecastDtoWrapper>(dto);
            }

            public string ProjectHoursToString
            {
                get
                {
                    if (ForecastProjectHoursDtos == null || ForecastProjectHoursDtos.Count == 0)
                        return string.Empty;

                    return ForecastProjectHoursDtos.OrderBy(x => x.Project.Name).Aggregate(string.Empty, (current, projectHour) => current + (projectHour.Project.Name + projectHour.Hours));
                }
            }
        }
    }
}
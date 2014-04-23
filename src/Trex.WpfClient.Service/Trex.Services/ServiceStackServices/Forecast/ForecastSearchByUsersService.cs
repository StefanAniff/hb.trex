using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.ServiceStack;
using TrexSL.Web.ServiceStackServices.Forecast.Helpers;

namespace TrexSL.Web.ServiceStackServices.Forecast
{
    public class ForecastSearchByUsersService : NhServiceBasePost<ForecastSearchByUsersRequest>
    {
        private readonly IForecastMonthRepository _forecastMonthRepository;
        private readonly HolidaysByPeriodProvider _holidaysByPeriodProvider;
        private readonly IDomainSettings _domainSettings;

        public ForecastSearchByUsersService(IForecastMonthRepository forecastMonthRepository
            , HolidaysByPeriodProvider holidaysByPeriodProvider
            , IDomainSettings domainSettings)
        {
            _forecastMonthRepository = forecastMonthRepository;
            _holidaysByPeriodProvider = holidaysByPeriodProvider;
            _domainSettings = domainSettings;
        }

        protected override object Send(ForecastSearchByUsersRequest request)
        {
            var result = new ForecastSearchResponse
                {
                    ForecastMonths = GetForecasts(request),
                    Holidays = _holidaysByPeriodProvider.GetHolidays(request.ForecastMonth, request.ForecastYear),
                    ProjectForecastTypeId = _domainSettings.ProjectForecastTypeId,
                };

            return result;
        }

        private List<ForecastMonthDto> GetForecasts(ForecastSearchByUsersRequest request)
        {
            var result = _forecastMonthRepository
                            .GetByUsersAndMonth(request.UserIds, request.ForecastMonth, request.ForecastYear)
                            .ToList();

            return new List<ForecastMonthDto>(result.Select(ForecastMonthToDto));
        }

        private static ForecastMonthDto ForecastMonthToDto(ForecastMonth x)
        {
            return Mapper.Map<ForecastMonth, ForecastMonthDto>(x);
        }
    }
}
using System.Collections.Generic;
using AutoMapper;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.ServiceStack;
using System.Linq;
using TrexSL.Web.ServiceStackServices.Forecast.Helpers;

namespace TrexSL.Web.ServiceStackServices.Forecast
{
    public class ForecastSearchByRegistrationService : NhServiceBasePost<ForecastSearchByRegistrationRequest>
    {
        private readonly IForecastMonthRepository _forecastMonthRepository;
        private readonly ForecastMonthSearchCriteriaCollector _criteriaCollector;
        private readonly HolidaysByPeriodProvider _holidaysByPeriodProvider;
        private readonly IDomainSettings _domainSettings;

        public ForecastSearchByRegistrationService(IForecastMonthRepository forecastMonthRepository
            , ForecastMonthSearchCriteriaCollector criteriaCollector
            , HolidaysByPeriodProvider holidaysByPeriodProvider
            , IDomainSettings domainSettings)
        {
            _forecastMonthRepository = forecastMonthRepository;
            _criteriaCollector = criteriaCollector;
            _holidaysByPeriodProvider = holidaysByPeriodProvider;
            _domainSettings = domainSettings;
        }

        protected override object Send(ForecastSearchByRegistrationRequest request)
        {
            var result = new ForecastSearchResponse
                {
                    ForecastMonths = GetForecastMonths(request),
                    Holidays = _holidaysByPeriodProvider.GetHolidays(request.ForecastMonth, request.ForecastYear),
                    ProjectForecastTypeId = _domainSettings.ProjectForecastTypeId,
                };


            return result;
        }

        private List<ForecastMonthDto> GetForecastMonths(ForecastSearchByRegistrationRequest request)
        {            
            var criterias = _criteriaCollector.Collect(request).ToList();

            // If criterias is empty. Return empty list. We dont want to return the whole table
            if (!criterias.Any())
                return new List<ForecastMonthDto>();

            var result = _forecastMonthRepository.GetBySearchCriterias(criterias, request.ForecastMonth, request.ForecastYear);
            return new List<ForecastMonthDto>(result.Select(ForecastMonthToDto));
        }

        private static ForecastMonthDto ForecastMonthToDto(ForecastMonth x)
        {
            return Mapper.Map<ForecastMonth, ForecastMonthDto>(x);
        }
    }           
}
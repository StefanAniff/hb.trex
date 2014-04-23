using System;
using System.Collections.Generic;
using System.Linq;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.ServiceStack;
using TimeRegistrationTypeEnum = Trex.Server.Core.Model.TimeRegistrationTypeEnum;

namespace TrexSL.Web.ServiceStackServices.Forecast
{
    // IVA: Refactor. Introduce helpers for different statistics aspects
    public class ForecastStatisticsService : NhServiceBasePost<ForecastStatisticsRequest>
    {
        private readonly IDomainSettings _domainSettings;
        private readonly IForecastRepository _forecastRepo;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IForecastMonthRepository _forecastMonthRepository;

        public ForecastStatisticsService(IForecastRepository forecastRepository
            , IDomainSettings domainSettings
            , ITimeEntryRepository timeEntryRepository
            , IForecastMonthRepository forecastMonthRepository)
        {
            _forecastRepo = forecastRepository;
            _domainSettings = domainSettings;
            _timeEntryRepository = timeEntryRepository;
            _forecastMonthRepository = forecastMonthRepository;
        }

        private ForecastStatisticsResponse GetValue(ForecastStatisticsRequest request)
        {
            return new ForecastStatisticsResponse
            {
                ForecastStatistics = GetForecastStatistics(request.UserId, request.DisplayedMonth, request.DisplayedYear, request.Now, request.WorkPlanRealizedHourBillableOnly)
            };
        }

        private ForecastStatisticsDto GetForecastStatistics(int userId, int displayedMonth, int displayedYear, DateTime now, bool workPlanRealizedHourBillableOnly)
        {
            var result = new ForecastStatisticsDto();
            var startDate = new DateTime(displayedYear, displayedMonth, 1);
            var endDate = startDate.AddMonths(1);

            DateSpan currentYearDateSpan = DateSpan.YearDateSpan(now.Year);
            DateSpan next12MonthsDateSpan = DateSpan.Next12MonthsDatespan(now);
            result.Next12MonthsDateSpan = new DateSpanDto { From = next12MonthsDateSpan.From, To = next12MonthsDateSpan.To };
            DateSpan currentMonthDateSpan = DateSpan.CurrentMonthDateSpan(startDate);

            // Vacation
            int vacationTypeId = _domainSettings.VacationForecastTypeId;

            DateSpan currentVacationYearSpan = DateSpan.VacationCurrentPeriodDateSpan(now);
            result.PlannedVacationCurrentDateSpan = new DateSpanDto { From = currentVacationYearSpan.From, To = currentVacationYearSpan.To };

            DateSpan nextVacationYearSpan = DateSpan.VacationNextPeriodDateSpan(now);
            result.PlannedVacationNextDateSpan = new DateSpanDto { From = nextVacationYearSpan.From, To = nextVacationYearSpan.To };

            DateSpan vacationHeldToDateSpan = DateSpan.VacationPeriodUntilDateDateSpan(now);

            result.CurrentYearExternal = _forecastRepo.GetHourSumByCriteria(userId, false, currentYearDateSpan);
            result.CurrentYearInternal = _forecastRepo.GetHourSumByCriteria(userId, true, currentYearDateSpan);
            result.Next12MonthsExternal = _forecastRepo.GetHourSumByCriteria(userId, false, next12MonthsDateSpan);
            result.Next12MonthsInternal = _forecastRepo.GetHourSumByCriteria(userId, true, next12MonthsDateSpan);
            result.DisplayedMonthExternal = _forecastRepo.GetHourSumByCriteria(userId, false, currentMonthDateSpan);
            result.DisplayedMonthInternal = _forecastRepo.GetHourSumByCriteria(userId, true, currentMonthDateSpan);

            result.PlannedVacationCurrent = _forecastRepo.GetForecastCountByForecastType(userId, vacationTypeId, currentVacationYearSpan);
            result.PlannedVacationNext = _forecastRepo.GetForecastCountByForecastType(userId, vacationTypeId, nextVacationYearSpan);
            result.UsedVacationToDateCurrent = _forecastRepo.GetForecastCountByForecastType(userId, vacationTypeId, vacationHeldToDateSpan);


            var timeEntriesByPeriodAndUser = _timeEntryRepository.GetTimeEntriesByPeriodAndUser(userId, startDate, endDate)
                .Where(x => x.StartTime < endDate);
            if (workPlanRealizedHourBillableOnly)
            {
                timeEntriesByPeriodAndUser = timeEntriesByPeriodAndUser.Where(x => x.Billable);
            }

            var forecastMonth = _forecastMonthRepository.GetByUserAndMonth(userId, displayedMonth, displayedYear);
            var forecasts = forecastMonth != null ? forecastMonth.Forecasts.ToList() : new List<Trex.Server.Core.Model.Forecast.Forecast>();

            CreateStatisticsForCurrentMonth(timeEntriesByPeriodAndUser.ToList(), result, forecasts);

            return result;
        }

        private void CreateStatisticsForCurrentMonth(IEnumerable<Trex.Server.Core.Model.TimeEntry> timeEntriesByPeriodAndUser,
            ForecastStatisticsDto result, List<Trex.Server.Core.Model.Forecast.Forecast> forecasts)
        {
            var normalTimeEntries = timeEntriesByPeriodAndUser
                .Where(x => x.Task.TimeRegistrationTypeEnum == TimeRegistrationTypeEnum.Standard)
                .ToList();
            var realizedTotal = normalTimeEntries
                .Sum(x => x.TimeSpent);

            var forecastDates = forecasts
                .Where(x => x.ForecastType.SupportsProjectHours)
                .Select(x => x.Date)
                .Distinct().ToList();

            var forecastsToDate = forecastDates.Count(x => x.Date <= DateTime.Today);


            decimal forecastHours = forecasts
                .Where(x => x.Date <= DateTime.Now)
                .Sum(x => x.ProjectRegistrations.Sum(pRegistration => pRegistration.Hours));


            foreach (var timeEntry in normalTimeEntries)
            {
                if (timeEntry.Task.Project.Company.Internal)
                {
                    result.DisplayedMonthRealizedInternal += (decimal)timeEntry.TimeSpent;
                }
                else
                {
                    result.DisplayedMonthRealizedExternal += (decimal)timeEntry.TimeSpent;
                }
            }
            result.DisplayedMonthForecastTotal = result.DisplayedMonthExternal + result.DisplayedMonthInternal;
            result.DisplayedMonthRealizedTotal = result.DisplayedMonthRealizedExternal + result.DisplayedMonthRealizedInternal;

            //forecast average
            if (forecastsToDate != 0)
            {
                result.ForecastAverageHours = forecastHours / forecastsToDate;
            }

            //Realized Momentum
            if (forecastsToDate != 0)
            {
                result.RealizedMomentumHours = (decimal)(realizedTotal / forecastsToDate);

                // Can be zero if no forecasts exists but timeentry exists
                if (result.ForecastAverageHours != 0)
                    result.RealizedMomentumPercent = (result.RealizedMomentumHours / result.ForecastAverageHours) - 1;
                else
                    result.RealizedMomentumPercent = 0;
            }

            //projection
            if (forecastDates.Any())
            {
                result.CalculatedProjectionHours = result.RealizedMomentumHours * forecastDates.Count();

                // Can be zero if no forecasts exists but timeentry exists
                if (result.DisplayedMonthForecastTotal != 0)
                    result.CalculatedProjectionPercent = (result.CalculatedProjectionHours / result.DisplayedMonthForecastTotal) - 1;
                else
                    result.CalculatedProjectionPercent = 0;
            }

            //forecast success
            if (forecastHours != 0)
            {
                result.ForecastSucessRatePercent = (result.DisplayedMonthRealizedTotal / forecastHours) - 1;
                result.ForecastSucessRateHours = result.DisplayedMonthRealizedTotal - forecastHours;
            }
        }


        protected override object Send(ForecastStatisticsRequest request)
        {
            return GetValue(request);
        }
    }
}

using System;
using Trex.Common.DataTransferObjects;

namespace Trex.SmartClient.Forecast.ForecastStatistics.DesignData
{
    public class DesignForecastStatisticsTabViewModel : IForecastStatisticsTabViewModel
    {
        private readonly ForecastStatisticsDto _stats = new ForecastStatisticsDto
                                                    {
                                                        CurrentYearExternal = 111,
                                                        CurrentYearInternal = 222,
                                                        Next12MonthsDateSpan = new DateSpanDto { From = new DateTime(2013, 1, 1), To = new DateTime(2013,12,31) },
                                                        Next12MonthsExternal = 333,
                                                        Next12MonthsInternal = 4444,
                                                        DisplayedMonthDateSpan = new DateSpanDto { From = new DateTime(2013, 1, 1), To = new DateTime(2013, 1, 31) },
                                                        DisplayedMonthExternal = 60,
                                                        DisplayedMonthInternal = 70,
                                                        PlannedVacationCurrentDateSpan = new DateSpanDto { From = new DateTime(2013, 5, 1), To = new DateTime(2014, 4, 30) },
                                                        PlannedVacationCurrent = 15,
                                                        PlannedVacationNextDateSpan = new DateSpanDto { From = new DateTime(2014, 5, 1), To = new DateTime(2015, 4, 30) },
                                                        PlannedVacationNext = 2,
                                                        UsedVacationToDateCurrent = 16,
                                                        ForecastAverageHours = 7.5m,
                                                        RealizedMomentumHours = 5.0m,
                                                        RealizedMomentumPercent = -0.25m,
                                                        CalculatedProjectionHours =300,
                                                        CalculatedProjectionPercent = 0.9m,
                                                        ForecastSucessRateHours = 100,
                                                        ForecastSucessRatePercent = 0.1m,
                                                        DisplayedMonthForecastTotal = 200
                                                    };

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public ForecastStatisticsDto Statistics
        {
            get
            {
                return _stats;
            } 
            set {}
        }

        public int CurrentYear
        {
            get { return DateTime.Now.Year; }
        }

        public string Next12MonthsDateSpanString
        {
            get { return string.Format("{0} - {1}", _stats.Next12MonthsDateSpan.From, _stats.Next12MonthsDateSpan.To); }
        }

        public string CurrentVacationPeriodString
        {
            get { return string.Format("{0} - {1}", _stats.PlannedVacationCurrentDateSpan.From, _stats.PlannedVacationCurrentDateSpan.To); }
        }

        public string NextVacationperiodString
        {
            get { return string.Format("{0} - {1}", _stats.PlannedVacationNextDateSpan.From, _stats.PlannedVacationNextDateSpan.To); }
        }

        public bool IsBusy
        {
            get { return true; }
            set {}
        }
    }
}
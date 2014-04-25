using System;

namespace Trex.Common.DataTransferObjects
{
    public class ForecastStatisticsDto
    {
        public decimal CurrentYearExternal { get; set; }
        public decimal CurrentYearInternal { get; set; }

        public DateSpanDto Next12MonthsDateSpan { get; set; }
        public decimal Next12MonthsExternal { get; set; }
        public decimal Next12MonthsInternal { get; set; }

        public DateSpanDto DisplayedMonthDateSpan { get; set; }
        public decimal DisplayedMonthExternal { get; set; }
        public decimal DisplayedMonthInternal { get; set; }

        public decimal DisplayedMonthRealizedExternal { get; set; }
        public decimal DisplayedMonthRealizedInternal { get; set; }

        public DateSpanDto PlannedVacationCurrentDateSpan { get; set; }
        public decimal PlannedVacationCurrent { get; set; }

        public DateSpanDto PlannedVacationNextDateSpan { get; set; }
        public decimal PlannedVacationNext { get; set; }

        public decimal UsedVacationToDateCurrent { get; set; }

        #region DISABLED FOR H&B

        //public decimal ForecastAverageHours { get; set; }

        //public decimal RealizedMomentumHours { get; set; }
        //public decimal RealizedMomentumPercent { get; set; }

        //public decimal CalculatedProjectionHours { get; set; }
        //public decimal CalculatedProjectionPercent { get; set; }

        //public decimal ForecastSucessRateHours { get; set; }
        //public decimal ForecastSucessRatePercent { get; set; }

        //public decimal DisplayedMonthForecastTotal { get; set; }
        //public decimal DisplayedMonthRealizedTotal { get; set; }

        #endregion

    }

    public class DateSpanDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
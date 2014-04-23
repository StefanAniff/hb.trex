using System;

namespace Trex.Server.Core.Model.Forecast
{
    public class DateSpan
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public static DateSpan YearDateSpan(int year)
        {
            var from = new DateTime(year, 1, 1);
            return new DateSpan { From = from, To = EndOfMonth(new DateTime(year, 12, 1)) };
        }

        public static DateSpan Next12MonthsDatespan(DateTime date)
        {
            var from = FirstDayOfNextMonth(date);
            var to = EndOfMonth(from.AddYears(1).AddMonths(-1));
            return new DateSpan { From = from, To = to };
        }

        public static DateSpan CurrentMonthDateSpan(DateTime date)
        {
            var from = new DateTime(date.Year, date.Month, 1);
            return new DateSpan
            {
                From = from,
                To = EndOfMonth(from)
            };
        }

        public static DateTime FirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime FirstDayOfNextMonth(DateTime dateTime)
        {
            return FirstDayOfMonth(dateTime.AddMonths(1));
        }

        public static DateTime EndOfMonth(DateTime dateTime)
        {
            return FirstDayOfNextMonth(dateTime).AddDays(-1);
        }

        #region Vacation period

        private const int VacationPeriodMonthStart = 5;

        public static DateSpan VacationPeriodUntilDateDateSpan(DateTime now)
        {
            var startYear = now.Month < VacationPeriodMonthStart
                                ? now.Year - 1
                                : now.Year;
            return new DateSpan
                {
                    From = new DateTime(startYear, VacationPeriodMonthStart, 1),
                    To = now.Date
                };
        }

        public static DateSpan VacationCurrentPeriodDateSpan(DateTime now)
        {
            return Next12MonthsDatespan(now.Month < VacationPeriodMonthStart
                    ? new DateTime(now.Year - 1, VacationPeriodMonthStart - 1, 1)
                    : new DateTime(now.Year, VacationPeriodMonthStart - 1, 1));
        }

        public static DateSpan VacationNextPeriodDateSpan(DateTime now)
        {
            return Next12MonthsDatespan(now.Month < VacationPeriodMonthStart
                    ? new DateTime(now.Year, VacationPeriodMonthStart - 1, 1)
                    : new DateTime(now.Year + 1, VacationPeriodMonthStart - 1, 1));
        }

        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Trex.SmartClient.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToTimeString(this TimeSpan timeSpan)
        {
            var prefix = timeSpan.Ticks < 0 ? "-" : string.Empty;
            var totalHours = Math.Floor(timeSpan.TotalHours);
            return string.Format("{3}{0:00}:{1:00}:{2:00}", Math.Abs(totalHours), Math.Abs(timeSpan.Minutes), Math.Abs(timeSpan.Seconds), prefix);
        }

        public static string ToShortDateAndTimeString(this DateTime dateTime)
        {
            return string.Format("{0} {1}", dateTime.ToShortDateString(), dateTime.ToShortTimeString());
        }

        public static string ToDayAndMonth(this DateTime dateTime)
        {
            return string.Format("{0:00}-{1:00}", dateTime.Day, dateTime.Month);
        }

        public static DateTime FirstDayOfWeek(this DateTime dateTime)
        {
            var returnDate = dateTime;
            while (returnDate.Date.DayOfWeek != DayOfWeek.Monday)
            {
                returnDate = returnDate.AddDays(-1);
            }
            return returnDate;
        }

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime FirstDayOfPreviousMonth(this DateTime dateTime)
        {
            return dateTime.AddMonths(-1).FirstDayOfMonth();
        }

        public static DateTime FirstDayOfNextMonth(this DateTime dateTime)
        {
            return dateTime.AddMonths(1).FirstDayOfMonth();
        }

        public static IEnumerable<DateTime> CreateDatesForMonth(this DateTime date)
        {
            var dateInMonth = new DateTime(date.Year, date.Month, 1);
            while (dateInMonth.Month == date.Month)
            {
                yield return dateInMonth;
                dateInMonth = dateInMonth.AddDays(1);
            }
        }

        public static bool IsWeekend(this DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
        }

        public static int Weeknumber(this DateTime date, string cultureName)
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);
            return culture.Calendar.GetWeekOfYear(date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
        }

        public static int WeeknumberDk(this DateTime date)
        {
            return date.Weeknumber("da-DK");
        }
    }
}
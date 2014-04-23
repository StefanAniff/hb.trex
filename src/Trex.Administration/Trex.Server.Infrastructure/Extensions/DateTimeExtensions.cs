using System;

namespace Trex.Server.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime NextFirstOfMonth(this DateTime dateTime)
        {
            if (dateTime.Day != 1)
            {
                var nextMonth = dateTime.AddMonths(1);
                return new DateTime(nextMonth.Year, nextMonth.Month, 1);
            }

            return dateTime;
        }
    }
}
using System;

namespace Trex.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime NextFirstOfMonth(this DateTime dateTime)
        {
            if (dateTime.Day != 1)
            {
                return new DateTime(dateTime.Year, dateTime.AddMonths(1).Month, 1);
            }

            return dateTime;
        }
    }
}
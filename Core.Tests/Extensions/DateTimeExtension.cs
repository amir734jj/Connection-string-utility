using System;

namespace Core.Tests.Extensions
{
    public static class DateTimeExtension
    {
        public static bool SimpleEquals(this DateTime dateTime, DateTime other)
        {
            return dateTime.Year == other.Year
                   && dateTime.Month == other.Month
                   && dateTime.Day == other.Day
                   && dateTime.Hour == other.Hour
                   && dateTime.Minute == other.Minute
                   && dateTime.Second == other.Second;
        }
    }
}
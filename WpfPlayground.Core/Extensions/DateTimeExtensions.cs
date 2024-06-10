using System;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>The minimum SQL date time</summary>
        public static DateTime MinimumSQLDateTime = new DateTime(1753, 1, 1);
        /// <summary>The maximum SQL date time</summary>
        public static DateTime MaximumSQLDateTime = new DateTime(9999, 12, 31);

        /// <summary>To the start of minute.</summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime ToStartOfMinute(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0, dateTime.Kind);
        }

        /// <summary>To the date.</summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime ToDate(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }

        /// <summary>To the iso8601 format.</summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static string ToIso8601Format(this DateTime dateTime)
        {
            return dateTime.ToString("s");
        }

        /// <summary>Ends the of day.</summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime dt)
        {
            DateTime dateTime = dt.Date;
            dateTime = dateTime.AddDays(1.0);
            return dateTime.AddTicks(-1L);
        }

        /// <summary>Starts the of day.</summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime dt)
        {
            return dt.Date;
        }
    }
}

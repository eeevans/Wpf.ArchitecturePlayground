using System;

namespace WpfPlayground.Core.Extensions
{
    public static class DateExtensions
    {
        public static DateTime GetFirstDayOfNextQuarter(this DateTime fromDate)
        {
            var timeComponent = fromDate.TimeOfDay;
            var currentQuarter = (fromDate.Month - 1) / 3;      //0,1,2,3

            int monthComponent = (currentQuarter + 1) % 4 * 3 + 1;
            int yearComponent = currentQuarter == 3 ? fromDate.Year + 1 : fromDate.Year;        //if current is 4th quarter, go to next year
            return new DateTime(yearComponent, monthComponent, 1) + timeComponent;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronoMata.Scheduling
{
    public class DateUtilities
    {
        public static int GetWeeksBetween(DateTime startDate, DateTime endDate, DayOfWeek startDayOfWeek = DayOfWeek.Monday)
        {
            // normalize to the start date the weeks corresponding to the given dates
            var startFirstDayOfWeek = GetFirstDayOfWeek(startDate, startDayOfWeek);
            var endFirstDayOfWeek = GetFirstDayOfWeek(endDate, startDayOfWeek);

            // because dates are normalized, the span should be divisible by 7 (span.Days % 7 == 0)
            return (endFirstDayOfWeek - startFirstDayOfWeek).Days / 7;
        }

        public static DateTime GetFirstDayOfWeek(DateTime date, DayOfWeek startDayOfWeek)
        {
            int diff = (7 + (date.DayOfWeek - startDayOfWeek)) % 7;
            return date.AddDays(-1 * diff).Date;
        }
    }
}

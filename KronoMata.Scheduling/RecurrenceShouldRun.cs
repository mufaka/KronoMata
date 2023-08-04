using KronoMata.Model;

namespace KronoMata.Scheduling
{
    public class RecurrenceShouldRun : IShouldRun
    {
        public bool ShouldRun(DateTime currentDate, ScheduledJob scheduledJob)
        {
            if (!scheduledJob.IsEnabled) return false;
            if (scheduledJob.StartTime > currentDate) return false;

            if (scheduledJob.EndTime.HasValue)
            {
                if (scheduledJob.EndTime < currentDate) return false;
            }

            if (scheduledJob.Frequency == ScheduleFrequency.Month)
            {
                var shouldRunMonth = CheckMonthFrequency(currentDate, scheduledJob);
                if (!shouldRunMonth) return false;
            }

            if (scheduledJob.Frequency == ScheduleFrequency.Week)
            {
                var shouldRunWeek = CheckWeekFrequency(currentDate, scheduledJob);
                if (!shouldRunWeek) return false;
            }

            if (scheduledJob.Frequency == ScheduleFrequency.Day)
            {
                var shouldRunDay = CheckDayFrequency(currentDate, scheduledJob);
                if (!shouldRunDay) return false;
            }

            var validIncrements = new ValidIncrements(scheduledJob);

            // Only check the Day if DayOfWeeks isn't defined.
            if (validIncrements.DayOfWeeks.Count == 0)
            {
                if (!validIncrements.Days.Contains(currentDate.Day)) return false;
            }

            if (!validIncrements.Hours.Contains(currentDate.Hour)) return false;
            if (!validIncrements.Minutes.Contains(currentDate.Minute)) return false;
            if (validIncrements.DayOfWeeks.Count > 0)
            {
                if (!validIncrements.DayOfWeeks.Contains(currentDate.DayOfWeek.ToString())) return false;
            }

            return true;
        }

        private bool CheckMonthFrequency(DateTime currentDate, ScheduledJob scheduledJob)
        {
            // is this a valid month relative to StartTime and Interval of month?
            var monthsBetween = (currentDate.Month - scheduledJob.StartTime.Month) + 12 * (currentDate.Year - scheduledJob.StartTime.Year);

            if (monthsBetween > 0)
            {
                if (monthsBetween % scheduledJob.Interval != 0) return false;
            }

            return true;
        }

        private bool CheckWeekFrequency(DateTime currentDate, ScheduledJob scheduledJob)
        {
            var weeksBetween = DateUtilities.GetWeeksBetween(scheduledJob.StartTime, currentDate);

            if (weeksBetween > 0)
            {
                if (weeksBetween % scheduledJob.Interval != 0) return false;
            }

            return true;
        }

        private bool CheckDayFrequency(DateTime currentDate, ScheduledJob scheduledJob)
        {
            var daysBetween = (currentDate - scheduledJob.StartTime).Days;

            if (daysBetween > 0)
            {
                if (daysBetween % scheduledJob.Interval != 0) return false;
            }

            return true;
        }

        /// <summary>
        /// Contains a list of valid values for the defined schedule.
        /// 
        /// </summary>
        private class ValidIncrements
        {
            public ValidIncrements(ScheduledJob scheduledJob)
            {
                Days = new List<int> { scheduledJob.StartTime.Day };
                DayOfWeeks = new List<string>();
                Hours = new List<int> { scheduledJob.StartTime.Hour };
                Minutes = new List<int> { scheduledJob.StartTime.Minute };

                if (!String.IsNullOrEmpty(scheduledJob.Days))
                {
                    Days = scheduledJob.Days.Split(',').Select(int.Parse).ToList();
                }

                if (!String.IsNullOrEmpty(scheduledJob.DayOfWeeks))
                {
                    DayOfWeeks = scheduledJob.DayOfWeeks.Split(',').ToList();
                }

                if (!String.IsNullOrEmpty(scheduledJob.Hours))
                {
                    Hours = scheduledJob.Hours.Split(',').Select(int.Parse).ToList();
                }

                if (!String.IsNullOrEmpty(scheduledJob.Minutes))
                {
                    Minutes = scheduledJob.Minutes.Split(',').Select(int.Parse).ToList();
                }
            }

            public List<int> Days { get; set; }
            public List<string> DayOfWeeks { get; set; }
            public List<int> Hours { get; set; }
            public List<int> Minutes { get; set; }
        }
    }
}

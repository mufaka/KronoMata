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

            var validIncrements = new ValidIncrements(scheduledJob);

            if (scheduledJob.Frequency == ScheduleFrequency.Month)
            {
                var shouldRunMonth = CheckMonthFrequency(currentDate, scheduledJob, validIncrements);
                if (!shouldRunMonth) return false;
            }

            return true;
        }

        private bool CheckMonthFrequency(DateTime currentDate, ScheduledJob scheduledJob, ValidIncrements validIncrements)
        {
            // is this a valid month relative to StartTime and Interval of month?
            var monthsBetween = (currentDate.Month - scheduledJob.StartTime.Month) + 12 * (currentDate.Year - scheduledJob.StartTime.Year);

            if (monthsBetween > 0)
            {
                if (monthsBetween % scheduledJob.Interval != 0) return false;
            }

            if (!validIncrements.Days.Contains(currentDate.Day)) return false;
            if (!validIncrements.Hours.Contains(currentDate.Hour)) return false;
            if (!validIncrements.Minutes.Contains(currentDate.Minute)) return false;

            // TODO: is this necessary? probably will cause confusion.
            if (validIncrements.DayOfWeeks.Count > 0)
            {
                if (!validIncrements.DayOfWeeks.Contains(currentDate.DayOfWeek.ToString())) return false;
            }

            return true;
        }

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

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

            if (scheduledJob.Frequency == ScheduleFrequency.Hour)
            {
                var shouldRunHour = CheckHourFrequency(currentDate, scheduledJob);
                if (!shouldRunHour) return false;
            }

            if (scheduledJob.Frequency == ScheduleFrequency.Minute)
            {
                var shouldRunMinute = CheckMinuteFrequency(currentDate, scheduledJob);
                if (!shouldRunMinute) return false;
            }

            return CheckValidIncrements(currentDate, scheduledJob);
        }

        private static bool CheckValidIncrements(DateTime currentDate, ScheduledJob scheduledJob)
        {
            var validIncrements = new ValidIncrements(scheduledJob);

            if (validIncrements.DayOfWeeks.Count > 0)
            {
                if (!validIncrements.DayOfWeeks.Contains(currentDate.DayOfWeek.ToString())) return false;
            }

            if (!String.IsNullOrEmpty(scheduledJob.Days))
            {
                if (!validIncrements.Days.Contains(currentDate.Day)) return false;
            }
            else if (validIncrements.Days.Count > 0)
            {
                if (!validIncrements.Days.Contains(currentDate.Day)) return false;
            }

            if (!String.IsNullOrEmpty(scheduledJob.Hours))
            {
                if (!validIncrements.Hours.Contains(currentDate.Hour)) return false;
            }

            if (!String.IsNullOrEmpty(scheduledJob.Minutes))
            {
                if (!validIncrements.Minutes.Contains(currentDate.Minute)) return false;
            }

            if (validIncrements.Hours.Count > 0)
            {
                if (!validIncrements.Hours.Contains(currentDate.Hour)) return false;
            }

            if (validIncrements.Minutes.Count > 0)
            {
                if (!validIncrements.Minutes.Contains(currentDate.Minute)) return false;
            }

            return true;
        }

        private static bool CheckMonthFrequency(DateTime currentDate, ScheduledJob scheduledJob)
        {
            // is this a valid month relative to StartTime and Interval of month?
            var monthsBetween = (currentDate.Month - scheduledJob.StartTime.Month) + 12 * (currentDate.Year - scheduledJob.StartTime.Year);

            if (monthsBetween > 0)
            {
                if (monthsBetween % scheduledJob.Interval != 0) return false;
            }

            return true;
        }

        private static bool CheckWeekFrequency(DateTime currentDate, ScheduledJob scheduledJob)
        {
            var weeksBetween = DateUtilities.GetWeeksBetween(scheduledJob.StartTime, currentDate);

            if (weeksBetween > 0)
            {
                if (weeksBetween % scheduledJob.Interval != 0) return false;
            }

            return true;
        }

        private static bool CheckDayFrequency(DateTime currentDate, ScheduledJob scheduledJob)
        {
            var daysBetween = (currentDate - scheduledJob.StartTime).Days;

            if (daysBetween > 0)
            {
                if (daysBetween % scheduledJob.Interval != 0) return false;
            }

            return true;
        }

        private static bool CheckHourFrequency(DateTime currentDate, ScheduledJob scheduledJob)
        {
            var hoursBetween = (currentDate - scheduledJob.StartTime).Hours;

            if (hoursBetween > 0)
            {
                if (hoursBetween % scheduledJob.Interval != 0) return false;
            }

            return true;
        }

        private static bool CheckMinuteFrequency(DateTime currentDate, ScheduledJob scheduledJob)
        {
            var minutesBetween = (currentDate - scheduledJob.StartTime).Minutes;

            if (minutesBetween > 0)
            {
                if (minutesBetween % scheduledJob.Interval != 0) return false;
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

                    if (String.IsNullOrEmpty(scheduledJob.Days))
                    {
                        Days.Clear();
                    }
                }

                if (!String.IsNullOrEmpty(scheduledJob.Hours))
                {
                    Hours = scheduledJob.Hours.Split(',').Select(int.Parse).ToList();
                }

                if (!String.IsNullOrEmpty(scheduledJob.Minutes))
                {
                    Minutes = scheduledJob.Minutes.Split(',').Select(int.Parse).ToList();
                }

                // if frequency is minutes, hours and days don't matter
                if (scheduledJob.Frequency == ScheduleFrequency.Minute)
                {
                    Hours.Clear();
                    Days.Clear();
                    Minutes.Clear();
                }

                // if frequency is hours, days don't matter
                if (scheduledJob.Frequency == ScheduleFrequency.Hour)
                {
                    Days.Clear();
                    Hours.Clear();
                }
            }

            public List<int> Days { get; set; }
            public List<string> DayOfWeeks { get; set; }
            public List<int> Hours { get; set; }
            public List<int> Minutes { get; set; }
        }
    }
}

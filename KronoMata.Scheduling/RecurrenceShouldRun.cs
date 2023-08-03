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

            return true;
        }
    }
}

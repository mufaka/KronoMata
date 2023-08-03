using KronoMata.Model;

namespace KronoMata.Scheduling
{
    public interface IShouldRun
    {
        bool ShouldRun(DateTime currentDate, ScheduledJob scheduledJob);
    }
}

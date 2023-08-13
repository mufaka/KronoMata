using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class ScheduledJobViewModel : BaseViewModel
    {
        public List<ScheduledJob> ScheduledJobs { get; set; } = new List<ScheduledJob>();
    }
}

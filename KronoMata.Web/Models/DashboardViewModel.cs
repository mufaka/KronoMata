using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class DashboardViewModel : BaseViewModel
    {
        public List<Model.Host> Hosts { get; set; } = new List<Model.Host>();

        public List<ScheduledJob> ScheduledJobs { get; set; } = new List<ScheduledJob>();
       
    }
}

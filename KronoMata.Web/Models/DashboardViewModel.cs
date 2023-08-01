using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class DashboardViewModel : BaseViewModel
    {
        public List<PluginMetaData> Plugins { get; set; } = new List<PluginMetaData>();
        public List<Model.Host> Hosts { get; set; } = new List<Model.Host>();
        public List<ScheduledJob> ScheduledJobs { get; set; } = new List<ScheduledJob>();
        public List<JobHistory> JobHistories { get; set; } = new List<JobHistory>();

        // Is there a good pattern for composite objects? eg: JobHistory -> Host, ScheduledJob -> PluginMetaData
    }
}

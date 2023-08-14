using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class ScheduledJobSaveViewModel : BaseViewModel
    {
        public string ActionUrl { get; set; } = String.Empty;
        public List<PluginMetaData> Plugins { get; set; } = new List<PluginMetaData>();
        public List<Model.Host> Hosts { get; set; } = new List<Model.Host>();
        public ScheduledJob ScheduledJob { get; set; } = new ScheduledJob();
    }
}

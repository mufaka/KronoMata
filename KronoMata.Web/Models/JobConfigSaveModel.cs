namespace KronoMata.Web.Models
{
    public class JobConfigSaveModel
    {
        public int ScheduledJobId { get; set; }

        public List<PluginConfigValue> PluginConfigValues { get; set; } = new List<PluginConfigValue>();
    }
}

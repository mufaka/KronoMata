using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class ConfigureScheduledJobViewModel : BaseViewModel
    {
        public ConfigureScheduledJobViewModel() 
        { 
            ScheduledJob = new ScheduledJob();
            PluginConfigurations = new List<PluginConfiguration>();
            ConfigurationValues = new List<ConfigurationValue>();
        }

        public ScheduledJob ScheduledJob { get; set; }
        public List<PluginConfiguration> PluginConfigurations { get; set; }
        public List<ConfigurationValue> ConfigurationValues { get; set; }
    }
}

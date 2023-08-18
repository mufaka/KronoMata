using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class JobConfigViewModel
    {
        public JobConfigViewModel(PluginConfiguration pluginConfiguration, ConfigurationValue configurationValue)
        {
            PluginConfiguration = pluginConfiguration;
            ConfigurationValue = configurationValue;
        }

        public PluginConfiguration PluginConfiguration { get; set; }
        public ConfigurationValue ConfigurationValue { get; set; }
    }
}

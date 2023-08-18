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

        public string ControlId
        {
            get
            {
                return $"config-{PluginConfiguration.Id}-{ConfigurationValue.Id}";
            }
        }

        public List<string> SelectValues
        {
            get
            {
                var list = new List<string>();

                if (!string.IsNullOrEmpty(PluginConfiguration.SelectValues))
                {
                    list.AddRange(PluginConfiguration.SelectValues.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                }

                return list;
            }
        }
    }
}

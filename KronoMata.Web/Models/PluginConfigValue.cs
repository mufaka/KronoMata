namespace KronoMata.Web.Models
{
    public class PluginConfigValue
    {
        public int PluginConfigurationId { get; set; }
        public int ConfigurationValueId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}

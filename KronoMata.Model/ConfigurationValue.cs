namespace KronoMata.Model
{
    /// <summary>
    /// Plugins define their own configuration parameters. The values
    /// of these parameters are configurable per ScheduledJob in order
    /// to allow for multiple instances of a Plugin to be run for different
    /// scenarios.
    /// </summary>
    [Serializable]
    public class ConfigurationValue
    {
        /// <summary>
        /// The primary key for the GlobalConfiguration.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ScheduleJob for which this ConfigurationValue
        /// relates to.
        /// </summary>
        public int ScheduledJobId { get; set; }

        /// <summary>
        /// The PluginConfiguration for which this ConfigurationValue
        /// relates to.
        /// </summary>
        public int PluginConfigurationId { get; set; }

        /// <summary>
        /// The value of the ConfigurationValue.
        /// </summary>
        public string Value { get; set; } = String.Empty;

        /// <summary>
        /// The date the ConfigurationValue was inserted.
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// The most recent date the ConfigurationValue
        /// was saved.
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}

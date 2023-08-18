namespace KronoMata.Public
{
    /// <summary>
    /// A PluginParameter defines a configuration variable
    /// for the IPlugin implementation.
    /// </summary>
    [Serializable]
    public class PluginParameter
    {
        /// <summary>
        /// The DataType of this PluginParameter.
        /// </summary>
        public ConfigurationDataType DataType { get; set; }

        /// <summary>
        /// The name of the PluginParameter.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The description of the PluginParameter.
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Whether or not a value is required to be set
        /// for this PluginParameter when configuring
        /// a ScheduledJob.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// A comma separated list of allowed values. Used to populate
        /// DataType.Select and DataType.SelectMultiple values
        /// </summary>
        public string SelectValues { get; set; } = String.Empty;
    }
}

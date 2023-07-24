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
        public string Name { get; set; }

        /// <summary>
        /// The description of the PluginParameter.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Whether or not a value is required to be set
        /// for this PluginParameter when configuring
        /// a ScheduledJob.
        /// </summary>
        public bool IsRequired { get; set; }
    }
}

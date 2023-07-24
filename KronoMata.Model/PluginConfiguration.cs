namespace KronoMata.Model
{
    /// <summary>
    /// A PluginConfiguration contains the name and ConfigurationDataType
    /// for Plugin specific configuration.
    /// 
    /// Implementations of IPlugin define their own list of PluginConfiguration. 
    /// ConfigurationValues are defined when creating a ScheduledJob.
    /// </summary>
    [Serializable]
    public class PluginConfiguration
    {
        /// <summary>
        /// The primary key for the PluginConfiguration.
        /// </summary>
        public int Id { get; set; } 

        /// <summary>
        /// The Id of the PluginMetaData this PluginConfiguration
        /// is used for.
        /// </summary>
        public int PluginMetaDataId { get; set; }

        /// <summary>
        /// The DataType of this PluginConfiguration.
        /// </summary>
        public ConfigurationDataType DataType { get; set; }

        /// <summary>
        /// The Name of this PluginConfiguration.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Description of this PluginConfiguration.
        /// </summary>
        public string Description { get; set; } 

        /// <summary>
        /// Whether or not a ConfigurationValue is required to 
        /// be filled in for this PluginConfiguration.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// The date the PluginConfiguration was inserted.
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// The most recent date the PluginConfiguration
        /// was saved.
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}

namespace KronoMata.Model
{
    /// <summary>
    /// A global configuration value used for system configuration only. Plugins
    /// define their own configuration parameters but also have access to certain
    /// GlobalConfiguration values.
    /// </summary>
    [Serializable]
    public class GlobalConfiguration
    {
        /// <summary>
        /// The primary key for the GlobalConfiguration.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Category provides a means to group GlobalConfiguration values as 
        /// well as to prevent ambiguity between values having the same 
        /// name.
        /// </summary>
        public string Category { get; set; } = String.Empty;

        /// <summary>
        /// The name of the GlobalConfiguration.
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// The value of the GlobalConfiguration.
        /// </summary>
        public string Value { get; set; } = String.Empty;

        /// <summary>
        /// Whether or not this GlobalConfiguration is accessible to all
        /// plugins.
        /// </summary>
        public bool IsAccessibleToPlugins { get; set; }

        /// <summary>
        /// Whether or not this GlobalConfiguration should be masked when
        /// displayed in the UI.
        /// </summary>
        public bool IsMasked { get; set; }

        /// <summary>
        /// The date the GlobalConfiguration was inserted.
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// The most recent date the GlobalConfiguration
        /// was saved.
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}

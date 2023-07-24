namespace KronoMata.Model
{
    /// <summary>
    /// A PluginMetaData contains information that allows
    /// the runtime to dynamically load an IPlugin implementation.
    /// 
    /// A ScheduledJob defines which IPlugin to run by referencing
    /// the IPlugin's PluginMetaData.
    /// </summary>
    [Serializable]
    public class PluginMetaData
    {
        /// <summary>
        /// The primary key for the PluginMetaData.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Id of the Package that contains the IPlugin
        /// implementation.
        /// </summary>
        public int PackageId { get; set; }

        /// <summary>
        /// The Name of the IPlugin implementation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Description of the IPlugin implementation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Version of the IPlugin implementation.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The .NET Assembly Name that contains the
        /// IPlugin implementation.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// The fully qualified Class name of the IPlugin
        /// implementation.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// The date the PluginMetaData was inserted.
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// The most recent date the PluginMetaData
        /// was saved.
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}

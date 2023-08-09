namespace KronoMata.Public
{
    /// <summary>
    /// IPlugin defines a schedulable task executable.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// The name of the plugin
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A description for the plugin
        /// </summary>
        string Description { get; }

        /// <summary>
        /// A version of the plugin. It is important to have
        /// unique versions because of how the plugins are loaded
        /// for specific jobs.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// A list of PluginParameter used for plugin specific configuration. The
        /// values for these will be passed to the Execute method at run time.
        /// </summary>
        List<PluginParameter> Parameters { get; }

        /// <summary>
        /// Executes the IPlugin implementation.
        /// </summary>
        /// <param name="systemConfig">A Dictionary of IPlugin accessible system configuration values.</param>
        /// <param name="pluginConfig">A Dictionary of IPlugin-ScheduledJob specific configuration values.</param>
        /// <returns>A list of PluginResult for logging information about the execution of the IPlugin.</returns>
        List<PluginResult> Execute(Dictionary<string, string> systemConfig, 
            Dictionary<string, string> pluginConfig);
    }
}

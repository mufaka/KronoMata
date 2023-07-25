namespace KronoMata.Public
{
    /// <summary>
    /// A PluginResult returns information about
    /// the execution of an IPlugin implementation.
    /// 
    /// PluginResults are mapped to JobHistory for 
    /// logging purposes.
    /// </summary>
    [Serializable]
    public class PluginResult
    {
        /// <summary>
        /// Whether or not this result represents that
        /// an error occured.
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// A summary message describing the result of the
        /// PluginResult execution.
        /// </summary>
        public string Message { get; set; } = String.Empty;

        /// <summary>
        /// A detailed message describing the result of the
        /// PluginResult execution.
        /// </summary>
        public string Detail { get; set; } = String.Empty;

    }
}

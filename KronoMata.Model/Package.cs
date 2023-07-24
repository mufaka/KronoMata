namespace KronoMata.Model
{
    /// <summary>
    /// A Package stores metadata about the location of a
    /// zip file that contains a Plugin binary.
    /// 
    /// Hosts running a Plugin for the first time will 
    /// download a Package locally in order to execute
    /// the Plugin.
    /// </summary>
    [Serializable]
    public class Package
    {
        /// <summary>
        /// The primary key for the Package.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the file containing the Package. It
        /// is assumed that the API and Agent will manage their
        /// own paths for Packages so only the FileName is
        /// required.
        /// </summary>
        public string FileName { get; set; }
    }
}

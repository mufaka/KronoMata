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
        /// A friendly name for the Package.
        /// </summary>
        public string Name { get; set; } = String.Empty; 

        /// <summary>
        /// The name of the zip file containing the Package. The
        /// value should be unique.
        /// </summary>
        public string FileName { get; set; } = String.Empty;

        /// <summary>
        /// The date and time that the Package was uploaded.
        /// </summary>
        public DateTime UploadDate { get; set; }
    }
}

namespace KronoMata.Model
{
    /// <summary>
    /// A Host represents a system that can run a ScheduledJob. ScheduledJobs 
    /// can be created to only run on specific Hosts or all enabled Hosts.
    /// 
    /// When an Agent is deployed to a system it should register
    /// the system as a Host using Environment.MachineName as 
    /// the MachineName value. This allows for the system to retrieve, by
    /// MachineName, the ScheduledJobs it should consider running.
    /// </summary>
    [Serializable]
    public class Host
    {
        /// <summary>
        /// The primary key for the Host.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Environment.MachineName value for the 
        /// host running the Agent.
        /// </summary>
        public string MachineName { get; set; } = String.Empty;

        /// <summary>
        /// Whether or not the Host is enabled to 
        /// run ScheduledJobs.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// The date the Host was inserted.
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// The most recent date the Host
        /// was saved.
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}

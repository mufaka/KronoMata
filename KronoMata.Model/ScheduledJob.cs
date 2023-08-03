namespace KronoMata.Model
{
    [Serializable]
    public class ScheduledJob
    {
        /// <summary>
        /// The primary key for the ScheduledJob.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Id of the PluginMetaData to use when
        /// dynamically loading an IPlugin implementation
        /// to run.
        /// </summary>
        public int PluginMetaDataId { get; set; }

        /// <summary>
        /// The Id of the Host to run the IPlugin implementation
        /// on. A Null value means that the IPlugin implementation
        /// will run on all enabled Agents.
        /// </summary>
        public int? HostId { get; set; }

        /// <summary>
        /// The name of the ScheduledJob.
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// The Description of the ScheduledJob.
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// The Frequency in which to run.
        /// </summary>
        public ScheduleFrequency Frequency { get; set; }

        /// <summary>
        /// The amount of intervals to skip before
        /// the next run.
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// A comma separated list of days to run on. Only
        /// relevant for ScheduleFrequency.Month
        /// </summary>
        public string Days { get; set; }

        /// <summary>
        /// A comma separated list of hours to run on. Not
        /// relevant for ScheduleFrequency.Hour or ScheduleFrequency.Minute
        /// </summary>
        public string Hours { get; set; }

        /// <summary>
        /// A comma separated list of minutes to run on. Not relevant
        /// for ScheduleFrequency.Minute.
        /// </summary>
        public string Minutes { get; set; }

        /// <summary>
        /// The Date and Time in which to start considering this
        /// ScheduledJob for execution. 
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The Date and Time in which to stop considering this
        /// ScheduledJob for execution. 
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Whether or not the ScheduledJob is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// The date the ScheduledJob was inserted.
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// The most recent date the ScheduledJob
        /// was saved.
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}

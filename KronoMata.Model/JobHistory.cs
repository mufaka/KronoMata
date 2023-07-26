namespace KronoMata.Model
{
    /// <summary>
    /// A JobHistory records the results of a Plugin execution on a particular
    /// Host for a particular ScheduledJob.
    /// </summary>
    [Serializable]
    public class JobHistory
    {
        /// <summary>
        /// The primary key for the JobHistory.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Id of the ScheduledJob that was run.
        /// </summary>
        public int ScheduledJobId { get; set; }

        /// <summary>
        /// The Id of the Host that ran the ScheduledJob
        /// </summary>
        public int HostId { get; set; }

        /// <summary>
        /// The resulting status of the ScheduledJob execution.
        /// </summary>
        public ScheduledJobStatus Status { get; set; }

        /// <summary>
        /// A summary message describing the result of the
        /// ScheduledJob execution.
        /// </summary>
        public string Message { get; set; } = String.Empty;

        /// <summary>
        /// A detailed message describing the result of the
        /// ScheduledJob execution.
        /// </summary>
        public string? Detail { get; set; }

        /// <summary>
        /// The execution time of the ScheduledJob.
        /// </summary>
        public DateTime RunTime { get; set; }
    }
}

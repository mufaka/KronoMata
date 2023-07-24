namespace KronoMata.Model
{
    /// <summary>
    /// An enumeration that defines the resulting status
    /// of a ScheduledJob execution.
    /// </summary>
    public enum ScheduledJobStatus
    {
        /// <summary>
        /// The ScheduledJob ran successfully.
        /// </summary>
        Success,
        /// <summary>
        /// The ScheduledJob was skipped.
        /// </summary>
        Skipped,
        /// <summary>
        /// The ScheduledJob failed.
        /// </summary>
        Failure
    }
}

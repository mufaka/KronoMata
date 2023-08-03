namespace KronoMata.Model
{
    /// <summary>
    /// A ScheduleInterval defines the interval
    /// at which a ScheduledJob should run.
    /// </summary>
    [Serializable]
    public enum ScheduleFrequency
    {
        Minute,
        Hour,
        Day,
        Week,
        Month
    }
}
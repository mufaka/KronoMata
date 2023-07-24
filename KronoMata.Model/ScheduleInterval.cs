namespace KronoMata.Model
{
    /// <summary>
    /// A ScheduleInterval defines the interval
    /// at which a ScheduledJob should run.
    /// </summary>
    [Serializable]
    public enum ScheduleInterval
    {
        Minute,
        Hour,
        Day,
        Week,
        Month
    }
}
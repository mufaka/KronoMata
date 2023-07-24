namespace KronoMata.Model
{
    /// <summary>
    /// A ConfigurationDataType defines the data type of
    /// a particular PluginConfiguration.
    /// 
    /// The ConfigurationDataType is useful for determining
    /// the UI element to present when editing ConfigurationValues.
    /// </summary>
    [Serializable]
    public enum ConfigurationDataType
    {
        String,
        Integer,
        Decimal,
        DateTime,
        Password,
        Text,
        Boolean
    }
}

namespace KronoMata.Data
{
    /// <summary>
    /// An IDataStoreProvider implementation provides access to
    /// all implementation specfic data stores. This allows for
    /// defining 1 or more implementations that support caching,
    /// different RDBMSs, API, file system, et al.
    /// 
    /// NOTE: Provider is used in lieu of dependency injection at this 
    /// level to reduce configuration complexity. 
    /// </summary>
    public interface IDataStoreProvider
    {
        IConfigurationValueDataStore ConfigurationValueDataStore { get; }
        IGlobalConfigurationDataStore GlobalConfigurationDataStore { get; }
        IHostDataStore HostDataStore { get; }
        IJobHistoryDataStore JobHistoryDataStore { get; }
        IPackageDataStore PackageDataStore { get; }
        IPluginConfigurationDataStore PluginConfigurationDataStore { get; }
        IPluginMetaDataDataStore PluginMetaDataDataStore { get; }
        IScheduledJobDataStore ScheduledJobDataStore { get; }
    }
}

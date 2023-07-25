namespace KronoMata.Data.Mock
{
    public class MockDataStoreProvider : IDataStoreProvider
    {
        
        public IConfigurationValueDataStore ConfigurationValueDataStore { get; } = new MockConfigurationValueDataStore();

        public IGlobalConfigurationDataStore GlobalConfigurationDataStore { get; } = new MockGlobalConfigurationDataStore();

        public IHostDataStore HostDataStore { get; } = new MockHostDataStore();

        public IJobHistoryDataStore JobHistoryDataStore { get; } = new MockJobHistoryDataStore();

        public IPackageDataStore PackageDataStore { get; } = new MockPackageDataStore();

        public IPluginConfigurationDataStore PluginConfigurationDataStore { get; } = new MockPluginConfigurationDataStore();

        public IPluginMetaDataDataStore PluginMetaDataDataStore { get; } = new MockPluginMetaDataDataStore();

        public IScheduledJobDataStore ScheduledJobDataStore { get; } = new MockScheduledJobDataStore();
    }
}

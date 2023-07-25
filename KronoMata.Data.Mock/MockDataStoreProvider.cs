namespace KronoMata.Data.Mock
{
    public class MockDataStoreProvider : IDataStoreProvider
    {
        public IConfigurationValueDataStore ConfigurationValueDataStore => new MockConfigurationValueDataStore();

        public IGlobalConfigurationDataStore GlobalConfigurationDataStore => new MockGlobalConfigurationDataStore();

        public IHostDataStore HostDataStore => new MockHostDataStore();

        public IJobHistoryDataStore JobHistoryDataStore => new MockJobHistoryDataStore();

        public IPackageDataStore PackageDataStore => new MockPackageDataStore();

        public IPluginConfigurationDataStore PluginConfigurationDataStore => new MockPluginConfigurationDataStore();

        public IPluginMetaDataDataStore PluginMetaDataDataStore => new MockPluginMetaDataDataStore();

        public IScheduledJobDataStore ScheduledJobDataStore => new MockScheduledJobDataStore();
    }
}

using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockDataStoreProvider : IDataStoreProvider
    {
        public MockDataStoreProvider() 
        {
            ConfigurationValueDataStore = new MockConfigurationValueDataStore(this);
            GlobalConfigurationDataStore = new MockGlobalConfigurationDataStore(this);
            HostDataStore = new MockHostDataStore(this);
            JobHistoryDataStore = new MockJobHistoryDataStore(this);
            PackageDataStore = new MockPackageDataStore(this);
            PluginConfigurationDataStore = new MockPluginConfigurationDataStore(this);
            PluginMetaDataDataStore = new MockPluginMetaDataDataStore(this);
            ScheduledJobDataStore = new MockScheduledJobDataStore(this);
        }
        
        public IConfigurationValueDataStore ConfigurationValueDataStore { get; private set; }

        public IGlobalConfigurationDataStore GlobalConfigurationDataStore { get; private set; }

        public IHostDataStore HostDataStore { get; private set; }

        public IJobHistoryDataStore JobHistoryDataStore { get; private set; }

        public IPackageDataStore PackageDataStore { get; private set; }

        public IPluginConfigurationDataStore PluginConfigurationDataStore { get; private set; }

        public IPluginMetaDataDataStore PluginMetaDataDataStore { get; private set; }

        public IScheduledJobDataStore ScheduledJobDataStore { get; private set; }
    }
}

using KronoMata.Data.Mock;

namespace KronoMata.Data.InMemory
{
    public class InMemoryDataStoreProvider : IDataStoreProvider
    {
        public InMemoryDataStoreProvider(MockDataStoreProvider inMemoryDataStoreProvider, IDataStoreProvider backingDataStoreProvider)
        {
            ConfigurationValueDataStore = new InMemoryConfigurationValueDataStore(inMemoryDataStoreProvider, backingDataStoreProvider);
            GlobalConfigurationDataStore = new InMemoryGlobalConfigurationDataStore(inMemoryDataStoreProvider, backingDataStoreProvider);
            HostDataStore = new InMemoryHostDataStore(inMemoryDataStoreProvider, backingDataStoreProvider);
            JobHistoryDataStore = new InMemoryJobHistoryDataStore(inMemoryDataStoreProvider, backingDataStoreProvider);
            PackageDataStore = new InMemoryPackageDataStore(inMemoryDataStoreProvider, backingDataStoreProvider);
            PluginConfigurationDataStore = new InMemoryPluginConfigurationDataStore(inMemoryDataStoreProvider, backingDataStoreProvider);
            PluginMetaDataDataStore = new InMemoryPluginMetaDataDataStore(inMemoryDataStoreProvider, backingDataStoreProvider);
            ScheduledJobDataStore = new InMemoryScheduledJobDataStore(inMemoryDataStoreProvider, backingDataStoreProvider);
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

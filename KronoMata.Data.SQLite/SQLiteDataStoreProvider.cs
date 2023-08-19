namespace KronoMata.Data.SQLite
{
    public class SQLiteDataStoreProvider : IDataStoreProvider
    {
        public SQLiteDataStoreProvider()
        {
            ConfigurationValueDataStore = new SQLiteConfigurationValueDataStore();
            GlobalConfigurationDataStore = new SQLiteGlobalConfigurationDataStore();
            HostDataStore = new SQLiteHostDataStore();
            JobHistoryDataStore = new SQLiteJobHistoryDataStore();
            PackageDataStore = new SQLitePackageDataStore();
            PluginConfigurationDataStore = new SQLitePluginConfigurationDataStore();
            PluginMetaDataDataStore = new SQLitePluginMetaDataDataStore();
            ScheduledJobDataStore = new SQLiteScheduledJobDataStore();
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

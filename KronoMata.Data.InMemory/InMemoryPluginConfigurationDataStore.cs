using KronoMata.Data.Mock;
using KronoMata.Model;

namespace KronoMata.Data.InMemory
{
    public class InMemoryPluginConfigurationDataStore : InMemoryDataStoreBase, IPluginConfigurationDataStore
    {
        public InMemoryPluginConfigurationDataStore(MockDataStoreProvider inMemoryDataStoreProvider, IDataStoreProvider backingDataStoreProvider)
            : base(inMemoryDataStoreProvider, backingDataStoreProvider) 
        {
            ((MockPluginConfigurationDataStore)inMemoryDataStoreProvider.PluginConfigurationDataStore)
                .Initialize(backingDataStoreProvider.PluginConfigurationDataStore.GetAll());
        }

        public PluginConfiguration Create(PluginConfiguration pluginConfiguration)
        {
            var createdPluginConfiguration = BackingDataStoreProvider.PluginConfigurationDataStore.Create(pluginConfiguration);
            InMemoryDataStoreProvider.PluginConfigurationDataStore.Create(createdPluginConfiguration);
            return createdPluginConfiguration;
        }

        public void Delete(int id)
        {
            BackingDataStoreProvider.PluginConfigurationDataStore.Delete(id);
            InMemoryDataStoreProvider.PluginConfigurationDataStore.Delete(id);
        }

        public PluginConfiguration GetById(int id)
        {
            return InMemoryDataStoreProvider.PluginConfigurationDataStore.GetById(id);
        }

        public List<PluginConfiguration> GetByPluginMetaData(int pluginMetaDataId)
        {
            return InMemoryDataStoreProvider.PluginConfigurationDataStore.GetByPluginMetaData(pluginMetaDataId);
        }

        public List<PluginConfiguration> GetAll()
        {
            return InMemoryDataStoreProvider.PluginConfigurationDataStore.GetAll();
        }

        public void Update(PluginConfiguration pluginConfiguration)
        {
            BackingDataStoreProvider.PluginConfigurationDataStore.Update(pluginConfiguration);
            InMemoryDataStoreProvider.PluginConfigurationDataStore.Update(pluginConfiguration);
        }
    }
}

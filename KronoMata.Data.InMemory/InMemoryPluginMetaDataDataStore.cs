using KronoMata.Data.Mock;
using KronoMata.Model;

namespace KronoMata.Data.InMemory
{
    public class InMemoryPluginMetaDataDataStore : InMemoryDataStoreBase, IPluginMetaDataDataStore
    {
        public InMemoryPluginMetaDataDataStore(MockDataStoreProvider inMemoryDataStoreProvider, IDataStoreProvider backingDataStoreProvider)
            : base(inMemoryDataStoreProvider, backingDataStoreProvider) 
        {
            ((MockPluginMetaDataDataStore)inMemoryDataStoreProvider.PluginMetaDataDataStore)
                .Initialize(backingDataStoreProvider.PluginMetaDataDataStore.GetAll());
        }

        public PluginMetaData Create(PluginMetaData pluginMetaData)
        {
            var createdPluginMetaData = BackingDataStoreProvider.PluginMetaDataDataStore.Create(pluginMetaData);
            InMemoryDataStoreProvider.PluginMetaDataDataStore.Create(createdPluginMetaData);
            return createdPluginMetaData;
        }

        public void Delete(int id)
        {
            BackingDataStoreProvider.PluginMetaDataDataStore.Delete(id);
            InMemoryDataStoreProvider.PluginMetaDataDataStore.Delete(id);
        }

        public PluginMetaData GetById(int id)
        {
            return InMemoryDataStoreProvider.PluginMetaDataDataStore.GetById(id);
        }

        public List<PluginMetaData> GetByPackageId(int packageId)
        {
            return InMemoryDataStoreProvider.PluginMetaDataDataStore.GetByPackageId(packageId);
        }

        public void Update(PluginMetaData pluginMetaData)
        {
            BackingDataStoreProvider.PluginMetaDataDataStore.Update(pluginMetaData);
            InMemoryDataStoreProvider.PluginMetaDataDataStore.Update(pluginMetaData);
        }

        public List<PluginMetaData> GetAll()
        {
            return InMemoryDataStoreProvider.PluginMetaDataDataStore.GetAll();
        }
    }
}

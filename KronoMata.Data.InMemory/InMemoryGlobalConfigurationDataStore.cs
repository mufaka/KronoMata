using KronoMata.Data.Mock;
using KronoMata.Model;

namespace KronoMata.Data.InMemory
{
    public class InMemoryGlobalConfigurationDataStore : InMemoryDataStoreBase, IGlobalConfigurationDataStore
    {
        public InMemoryGlobalConfigurationDataStore(MockDataStoreProvider inMemoryDataStoreProvider, IDataStoreProvider backingDataStoreProvider)
            : base(inMemoryDataStoreProvider, backingDataStoreProvider) 
        {
            ((MockGlobalConfigurationDataStore)inMemoryDataStoreProvider.GlobalConfigurationDataStore)
                .Initialize(backingDataStoreProvider.GlobalConfigurationDataStore.GetAll());
        }

        public GlobalConfiguration Create(GlobalConfiguration globalConfiguration)
        {
            var createdGlobalConfiguration = BackingDataStoreProvider.GlobalConfigurationDataStore.Create(globalConfiguration);
            BackingDataStoreProvider.GlobalConfigurationDataStore.Create(createdGlobalConfiguration);
            return createdGlobalConfiguration;
        }

        public void Delete(int id)
        {
            BackingDataStoreProvider.GlobalConfigurationDataStore.Delete(id);
            InMemoryDataStoreProvider.GlobalConfigurationDataStore.Delete(id);
        }

        public GlobalConfiguration GetById(int id)
        {
            return InMemoryDataStoreProvider.GlobalConfigurationDataStore.GetById(id);
        }

        public List<GlobalConfiguration> GetAll()
        {
            return InMemoryDataStoreProvider.GlobalConfigurationDataStore.GetAll();
        }

        public List<GlobalConfiguration> GetByCategory(string categoryName)
        {
            return InMemoryDataStoreProvider.GlobalConfigurationDataStore.GetByCategory(categoryName);
        }

        public GlobalConfiguration GetByCategoryAndName(string category, string name)
        {
            return InMemoryDataStoreProvider.GlobalConfigurationDataStore.GetByCategoryAndName(category, name);
        }

        public void Update(GlobalConfiguration globalConfiguration)
        {
            BackingDataStoreProvider.GlobalConfigurationDataStore.Update(globalConfiguration);
            InMemoryDataStoreProvider.GlobalConfigurationDataStore.Update(globalConfiguration);
        }
    }
}

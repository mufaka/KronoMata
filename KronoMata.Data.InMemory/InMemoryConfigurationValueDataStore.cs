using KronoMata.Data.Mock;
using KronoMata.Model;

namespace KronoMata.Data.InMemory
{
    public class InMemoryConfigurationValueDataStore : InMemoryDataStoreBase, IConfigurationValueDataStore
    {
        public InMemoryConfigurationValueDataStore(MockDataStoreProvider inMemoryDataStoreProvider, IDataStoreProvider backingDataStoreProvider) 
            : base(inMemoryDataStoreProvider, backingDataStoreProvider) 
        {
            ((MockConfigurationValueDataStore)inMemoryDataStoreProvider.ConfigurationValueDataStore)
                .Initialize(backingDataStoreProvider.ConfigurationValueDataStore.GetAll());
        }

        public ConfigurationValue Create(ConfigurationValue configurationValue)
        {
            var createdConfigurationValue = BackingDataStoreProvider.ConfigurationValueDataStore.Create(configurationValue);
            InMemoryDataStoreProvider.ConfigurationValueDataStore.Create(createdConfigurationValue);
            return createdConfigurationValue;
        }

        public void Delete(int id)
        {
            BackingDataStoreProvider.ConfigurationValueDataStore.Delete(id);
            InMemoryDataStoreProvider.ConfigurationValueDataStore.Delete(id);
        }

        public List<ConfigurationValue> GetByScheduledJob(int scheduledJobId)
        {
            return InMemoryDataStoreProvider.ConfigurationValueDataStore.GetByScheduledJob(scheduledJobId);
        }

        public List<ConfigurationValue> GetAll()
        {
            return InMemoryDataStoreProvider.ConfigurationValueDataStore.GetAll();
        }

        public void Update(ConfigurationValue configurationValue)
        {
            BackingDataStoreProvider.ConfigurationValueDataStore.Update(configurationValue);
            InMemoryDataStoreProvider.ConfigurationValueDataStore.Update(configurationValue);
        }
    }
}

using KronoMata.Data.Mock;
using KronoMata.Model;

namespace KronoMata.Data.InMemory
{
    public class InMemoryScheduledJobDataStore : InMemoryDataStoreBase, IScheduledJobDataStore
    {
        public InMemoryScheduledJobDataStore(MockDataStoreProvider inMemoryDataStoreProvider, IDataStoreProvider backingDataStoreProvider)
            : base(inMemoryDataStoreProvider, backingDataStoreProvider) 
        {
            ((MockScheduledJobDataStore)inMemoryDataStoreProvider.ScheduledJobDataStore)
                .Initialize(backingDataStoreProvider.ScheduledJobDataStore.GetAll());
        }

        public ScheduledJob Create(ScheduledJob scheduledJob)
        {
            var createdScheduledJob = BackingDataStoreProvider.ScheduledJobDataStore.Create(scheduledJob);
            InMemoryDataStoreProvider.ScheduledJobDataStore.Create(createdScheduledJob);
            return createdScheduledJob;
        }

        public void Delete(int id)
        {
            BackingDataStoreProvider.ScheduledJobDataStore.Delete(id);
            InMemoryDataStoreProvider.ScheduledJobDataStore.Delete(id);
        }

        public List<ScheduledJob> GetAll()
        {
            return InMemoryDataStoreProvider.ScheduledJobDataStore.GetAll();
        }

        public List<ScheduledJob> GetByHost(int hostId)
        {
            return InMemoryDataStoreProvider.ScheduledJobDataStore.GetByHost(hostId);
        }

        public ScheduledJob GetById(int id)
        {
            return InMemoryDataStoreProvider.ScheduledJobDataStore.GetById(id);
        }

        public List<ScheduledJob> GetByPluginMetaData(int pluginMetaDataId)
        {
            return InMemoryDataStoreProvider.ScheduledJobDataStore.GetByPluginMetaData(pluginMetaDataId);
        }

        public void Update(ScheduledJob scheduledJob)
        {
            BackingDataStoreProvider.ScheduledJobDataStore.Update(scheduledJob);
            InMemoryDataStoreProvider.ScheduledJobDataStore.Update(scheduledJob);
        }
    }
}

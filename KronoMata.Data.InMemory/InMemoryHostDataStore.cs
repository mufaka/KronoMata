using KronoMata.Data.Mock;
using KronoMata.Model;

namespace KronoMata.Data.InMemory
{
    public class InMemoryHostDataStore : InMemoryDataStoreBase, IHostDataStore
    {
        public InMemoryHostDataStore(MockDataStoreProvider inMemoryDataStoreProvider, IDataStoreProvider backingDataStoreProvider)
            : base(inMemoryDataStoreProvider, backingDataStoreProvider)
        {
            ((MockHostDataStore)inMemoryDataStoreProvider.HostDataStore)
                .Initialize(backingDataStoreProvider.HostDataStore.GetAll());
        }

        public Host Create(Host host)
        {
            var createdHost = BackingDataStoreProvider.HostDataStore.Create(host);
            InMemoryDataStoreProvider.HostDataStore.Create(createdHost);
            return createdHost;
        }

        public void Delete(int id)
        {
            BackingDataStoreProvider.HostDataStore.Delete(id);
            InMemoryDataStoreProvider.HostDataStore.Delete(id);
        }

        public Host GetById(int id)
        {
            return InMemoryDataStoreProvider.HostDataStore.GetById(id);
        }

        public List<Host> GetAll()
        {
            return InMemoryDataStoreProvider.HostDataStore.GetAll();
        }

        public Host GetByMachineName(string machineName)
        {
            return InMemoryDataStoreProvider.HostDataStore.GetByMachineName(machineName);
        }

        public void Update(Host host)
        {
            BackingDataStoreProvider.HostDataStore.Update(host);
            InMemoryDataStoreProvider.HostDataStore.Update(host);
        }
    }
}

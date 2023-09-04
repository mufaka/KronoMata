using KronoMata.Data.Mock;

namespace KronoMata.Data.InMemory
{
    public abstract class InMemoryDataStoreBase
    {
        public MockDataStoreProvider InMemoryDataStoreProvider { get; private set; }
        public IDataStoreProvider BackingDataStoreProvider { get; private set; }

        public InMemoryDataStoreBase(MockDataStoreProvider inMemoryDataStoreProvider, IDataStoreProvider backingDataStoreProvider)
        {
            InMemoryDataStoreProvider = inMemoryDataStoreProvider;
            BackingDataStoreProvider = backingDataStoreProvider;
        }
    }
}

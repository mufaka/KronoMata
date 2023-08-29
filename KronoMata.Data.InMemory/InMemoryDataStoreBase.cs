using KronoMata.Data.Mock;

namespace KronoMata.Data.InMemory
{
    public abstract class InMemoryDataStoreBase
    {
        protected MockDataStoreProvider InMemoryDataStoreProvider { get; private set; }
        protected IDataStoreProvider BackingDataStoreProvider { get; private set; }

        public InMemoryDataStoreBase(MockDataStoreProvider inMemoryDataStoreProvider, IDataStoreProvider backingDataStoreProvider)
        {
            InMemoryDataStoreProvider = inMemoryDataStoreProvider;
            BackingDataStoreProvider = backingDataStoreProvider;
        }
    }
}

﻿namespace KronoMata.Data.Mock
{
    public abstract class BaseMockDataStore
    {
        // can/should this be IDataStoreProvider instead?
        public BaseMockDataStore(MockDataStoreProvider mockDataStoreProvider)
        {
            DataStoreProvider = mockDataStoreProvider;
        }

        public MockDataStoreProvider DataStoreProvider { get; private set; }
    }
}

using KronoMata.Data;
using KronoMata.Data.InMemory;
using KronoMata.Data.Mock;
using KronoMata.Data.SQLite;
using Test.KronoMata.Data.Base;

namespace Test.KronoMata.Data.Mock
{
    [TestFixture()]
    public class InMemoryJobHistoryDataStoreTests : JobHistoryDataStoreTestsBase
    {
        private IDataStoreProvider _provider;

        protected override IDataStoreProvider DataStoreProvider { get { return _provider; } }

        [SetUp]
        public void Setup()
        {
            var mockProvider = new MockDataStoreProvider();
            var sqliteProvider = new SQLiteDataStoreProvider();

            var databasePath = Path.Combine("Database", "KronoMata.db");
            SQLiteDataStoreBase.ConnectionString = $"Data Source={databasePath};Pooling=True;Cache Size=4000;Page Size=1024;FailIfMissing=True;Journal Mode=WAL;";

            _provider = new InMemoryDataStoreProvider(mockProvider, sqliteProvider);

            ClearTable();
        }

        [TearDown]
        public void ClearTable()
        {
            var inMemoryDataStore = (InMemoryJobHistoryDataStore)_provider.JobHistoryDataStore;
            ((SQLiteDataStoreBase)inMemoryDataStore.BackingDataStoreProvider.JobHistoryDataStore).TruncateTable("JobHistory");
        }
    }
}
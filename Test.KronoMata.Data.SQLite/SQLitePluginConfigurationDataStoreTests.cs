using KronoMata.Data;
using KronoMata.Data.SQLite;
using Test.KronoMata.Data.Base;

namespace Test.KronoMata.Data.SQLite
{
    [TestFixture()]
    public class SQLitePluginConfigurationDataStoreTests : PluginConfigurationDataStoreTestsBase
    {
        private IDataStoreProvider _provider;

        protected override IDataStoreProvider DataStoreProvider { get { return _provider; } }

        [SetUp]
        public void Setup()
        {
            _provider = new SQLiteDataStoreProvider();
            var databasePath = Path.Combine("Database", "KronoMata.db");
            SQLiteDataStoreBase.ConnectionString = $"Data Source={databasePath};Pooling=True;Cache Size=4000;Page Size=1024;FailIfMissing=True;Journal Mode=Off;";
            ClearTable();
        }

        [TearDown]
        public void ClearTable()
        {
            ((SQLiteDataStoreBase)_provider.ConfigurationValueDataStore).TruncateTable("PluginConfiguration");
        }
    }
}
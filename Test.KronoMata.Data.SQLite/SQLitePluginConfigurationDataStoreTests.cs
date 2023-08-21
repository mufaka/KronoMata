using KronoMata.Data;
using KronoMata.Data.SQLite;
using KronoMata.Model;
using KronoMata.Public;

namespace Test.KronoMata.Data.SQLite
{
    [TestFixture()]
    public class SQLitePluginConfigurationDataStoreTests
    {
        private IDataStoreProvider _provider;

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

        [Test()]
        public void Can_Create()
        {
            var now = DateTime.Now;

            var pluginConfiguration = new PluginConfiguration()
            {
                PluginMetaDataId = 1,
                DataType = ConfigurationDataType.Integer,
                Name = "PluginConfigurationName",
                Description = "PluginConfigurationDescription",
                IsRequired = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.PluginConfigurationDataStore.Create(pluginConfiguration);

            Assert.That(pluginConfiguration.Id, Is.EqualTo(1));
        }

        [Test()]
        public void Can_Delete()
        {
            var now = DateTime.Now;

            var pluginConfiguration = new PluginConfiguration()
            {
                PluginMetaDataId = 1,
                DataType = ConfigurationDataType.Integer,
                Name = "PluginConfigurationName",
                Description = "PluginConfigurationDescription",
                IsRequired = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.PluginConfigurationDataStore.Create(pluginConfiguration);

            Assert.That(pluginConfiguration.Id, Is.EqualTo(1));

            _provider.PluginConfigurationDataStore.Delete(pluginConfiguration.Id);

            var existing = _provider.PluginConfigurationDataStore.GetById(pluginConfiguration.Id);
            Assert.That(existing, Is.Null);
        }

        [Test()]
        public void Can_GetById()
        {
            var now = DateTime.Now;

            var pluginConfiguration = new PluginConfiguration()
            {
                PluginMetaDataId = 1,
                DataType = ConfigurationDataType.Integer,
                Name = "PluginConfigurationName",
                Description = "PluginConfigurationDescription",
                IsRequired = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.PluginConfigurationDataStore.Create(pluginConfiguration);

            Assert.That(pluginConfiguration.Id, Is.EqualTo(1));

            var existing = _provider.PluginConfigurationDataStore.GetById(1);

            Assert.That(existing, Is.Not.Null);
            Assert.That(existing.Id, Is.EqualTo(1));
        }

        [Test()]
        public void Can_GetByPluginMetaData()
        {
            var now = DateTime.Now;

            for (int x = 1; x <= 10; x++)
            {
                var pluginConfiguration1 = new PluginConfiguration()
                {
                    PluginMetaDataId = 1,
                    DataType = ConfigurationDataType.Integer,
                    Name = "PluginConfigurationName",
                    Description = "PluginConfigurationDescription",
                    IsRequired = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                _provider.PluginConfigurationDataStore.Create(pluginConfiguration1);

                var pluginConfiguration2 = new PluginConfiguration()
                {
                    PluginMetaDataId = 2,
                    DataType = ConfigurationDataType.Integer,
                    Name = "PluginConfigurationName",
                    Description = "PluginConfigurationDescription",
                    IsRequired = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                _provider.PluginConfigurationDataStore.Create(pluginConfiguration2);
            }

            var byPluginMetaDataList = _provider.PluginConfigurationDataStore.GetByPluginMetaData(1);
            Assert.That(byPluginMetaDataList, Has.Count.EqualTo(10));
        }

        [Test()]
        public void Can_Update()
        {
            var now = DateTime.Now;

            var pluginConfiguration = new PluginConfiguration()
            {
                PluginMetaDataId = 1,
                DataType = ConfigurationDataType.Integer,
                Name = "PluginConfigurationName",
                Description = "PluginConfigurationDescription",
                IsRequired = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.PluginConfigurationDataStore.Create(pluginConfiguration);

            Assert.That(pluginConfiguration.Id, Is.EqualTo(1));

            pluginConfiguration.Name = "UpdatedPluginConfigurationName";

            _provider.PluginConfigurationDataStore.Update(pluginConfiguration);

            var updated = _provider.PluginConfigurationDataStore.GetById(pluginConfiguration.Id);

            Assert.That(updated.Name, Is.EqualTo("UpdatedPluginConfigurationName"));
        }
    }
}
using KronoMata.Data;
using KronoMata.Data.Mock;
using KronoMata.Model;
using KronoMata.Public;

namespace Test.KronoMata.Data.Mock
{
    [TestFixture()]
    public class MockPluginConfigurationDataStoreTests
    {
        private IDataStoreProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new MockDataStoreProvider();
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

            Assert.IsTrue(1 == pluginConfiguration.Id);
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

            Assert.IsTrue(1 == pluginConfiguration.Id);

            _provider.PluginConfigurationDataStore.Delete(pluginConfiguration.Id);

            var existing = _provider.PluginConfigurationDataStore.GetById(pluginConfiguration.Id);
            Assert.IsNull(existing);
        }

        [Test()]
        public void Can_GetById()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_GetByPluginMetaData()
        {
            Assert.Fail();
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

            Assert.IsTrue(1 == pluginConfiguration.Id);

            pluginConfiguration.Name = "UpdatedPluginConfigurationName";

            _provider.PluginConfigurationDataStore.Update(pluginConfiguration);

            var updated = _provider.PluginConfigurationDataStore.GetById(pluginConfiguration.Id);

            Assert.That(updated.Name, Is.EqualTo("UpdatedPluginConfigurationName"));
        }
    }
}
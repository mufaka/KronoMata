using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Public;
using NUnit.Framework;

namespace Test.KronoMata.Data.Base
{
    [TestFixture]
    public abstract class PluginConfigurationDataStoreTestsBase
    {
        protected abstract IDataStoreProvider DataStoreProvider { get; }

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

            DataStoreProvider.PluginConfigurationDataStore.Create(pluginConfiguration);

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

            DataStoreProvider.PluginConfigurationDataStore.Create(pluginConfiguration);

            Assert.That(pluginConfiguration.Id, Is.EqualTo(1));

            DataStoreProvider.PluginConfigurationDataStore.Delete(pluginConfiguration.Id);

            var existing = DataStoreProvider.PluginConfigurationDataStore.GetById(pluginConfiguration.Id);
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

            DataStoreProvider.PluginConfigurationDataStore.Create(pluginConfiguration);

            Assert.That(pluginConfiguration.Id, Is.EqualTo(1));

            var existing = DataStoreProvider.PluginConfigurationDataStore.GetById(1);

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

                DataStoreProvider.PluginConfigurationDataStore.Create(pluginConfiguration1);

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

                DataStoreProvider.PluginConfigurationDataStore.Create(pluginConfiguration2);
            }

            var byPluginMetaDataList = DataStoreProvider.PluginConfigurationDataStore.GetByPluginMetaData(1);
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

            DataStoreProvider.PluginConfigurationDataStore.Create(pluginConfiguration);

            Assert.That(pluginConfiguration.Id, Is.EqualTo(1));

            pluginConfiguration.Name = "UpdatedPluginConfigurationName";

            DataStoreProvider.PluginConfigurationDataStore.Update(pluginConfiguration);

            var updated = DataStoreProvider.PluginConfigurationDataStore.GetById(pluginConfiguration.Id);

            Assert.That(updated.Name, Is.EqualTo("UpdatedPluginConfigurationName"));
        }
    }
}
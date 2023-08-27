using KronoMata.Data;
using KronoMata.Model;
using NUnit.Framework;

namespace Test.KronoMata.Data.Base
{
    [TestFixture]
    public abstract class PluginMetaDataDataStoreTestsBase
    {
        protected abstract IDataStoreProvider DataStoreProvider { get; }

        [Test()]
        public void Can_Create()
        {
            var now = DateTime.Now;

            var pluginMetaData = new PluginMetaData()
            {
                PackageId = 1,
                Name = "Name",
                Description = "Description",
                Version = "1.0",
                AssemblyName = "System",
                ClassName = "System.Object",
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.PluginMetaDataDataStore.Create(pluginMetaData);

            Assert.That(pluginMetaData.Id, Is.EqualTo(1));
        }

        [Test()]
        public void Can_Delete()
        {
            var now = DateTime.Now;

            var pluginMetaData = new PluginMetaData()
            {
                PackageId = 1,
                Name = "Name",
                Description = "Description",
                Version = "1.0",
                AssemblyName = "System",
                ClassName = "System.Object",
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.PluginMetaDataDataStore.Create(pluginMetaData);

            Assert.That(pluginMetaData.Id, Is.EqualTo(1));

            DataStoreProvider.PluginMetaDataDataStore.Delete(pluginMetaData.Id);

            var existing = DataStoreProvider.PluginMetaDataDataStore.GetById(pluginMetaData.Id);
            Assert.That(existing, Is.Null);
        }

        [Test()]
        public void Can_GetById()
        {
            var now = DateTime.Now;

            var pluginMetaData = new PluginMetaData()
            {
                PackageId = 1,
                Name = "Name",
                Description = "Description",
                Version = "1.0",
                AssemblyName = "System",
                ClassName = "System.Object",
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.PluginMetaDataDataStore.Create(pluginMetaData);

            Assert.That(pluginMetaData.Id, Is.EqualTo(1));

            var existing = DataStoreProvider.PluginMetaDataDataStore.GetById(1);

            Assert.That(existing, Is.Not.Null);
            Assert.That(existing.Id, Is.EqualTo(1));
        }

        [Test()]
        public void Can_GetByPackageId()
        {
            var now = DateTime.Now;

            for (int x = 1; x <= 10; x++)
            {
                var pluginMetaData1 = new PluginMetaData()
                {
                    PackageId = 1,
                    Name = "Name",
                    Description = "Description",
                    Version = "1.0",
                    AssemblyName = "System",
                    ClassName = "System.Object",
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.PluginMetaDataDataStore.Create(pluginMetaData1);

                var pluginMetaData2 = new PluginMetaData()
                {
                    PackageId = 2,
                    Name = "Name",
                    Description = "Description",
                    Version = "1.0",
                    AssemblyName = "System",
                    ClassName = "System.Object",
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.PluginMetaDataDataStore.Create(pluginMetaData2);
            }

            var byPackageIdList = DataStoreProvider.PluginMetaDataDataStore.GetByPackageId(2);

            Assert.That(byPackageIdList, Has.Count.EqualTo(10));
        }

        [Test()]
        public void Can_Update()
        {
            var now = DateTime.Now;

            var pluginMetaData = new PluginMetaData()
            {
                PackageId = 1,
                Name = "Name",
                Description = "Description",
                Version = "1.0",
                AssemblyName = "System",
                ClassName = "System.Object",
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.PluginMetaDataDataStore.Create(pluginMetaData);

            Assert.That(pluginMetaData.Id, Is.EqualTo(1));

            pluginMetaData.Description = "UpdatedDescription";

            DataStoreProvider.PluginMetaDataDataStore.Update(pluginMetaData);

            var updated = DataStoreProvider.PluginMetaDataDataStore.GetById(pluginMetaData.Id);

            Assert.That(updated.Description, Is.EqualTo("UpdatedDescription"));
        }
    }
}
using KronoMata.Data;
using KronoMata.Data.Mock;
using KronoMata.Model;

namespace Test.KronoMata.Data.Mock
{
    [TestFixture()]
    public class MockPluginMetaDataDataStoreTests
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

            var pluginMetaData = new PluginMetaData()
            {
                PackageId = 1,
                Name = "Name",
                Description  = "Description",
                Version = "1.0",
                AssemblyName = "System",
                ClassName = "System.Object",
                InsertDate = now,
                UpdateDate = now
            };

            _provider.PluginMetaDataDataStore.Create(pluginMetaData);

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

            _provider.PluginMetaDataDataStore.Create(pluginMetaData);

            Assert.That(pluginMetaData.Id, Is.EqualTo(1));

            _provider.PluginMetaDataDataStore.Delete(pluginMetaData.Id);

            var existing = _provider.PluginMetaDataDataStore.GetById(pluginMetaData.Id);
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

            _provider.PluginMetaDataDataStore.Create(pluginMetaData);

            Assert.That(pluginMetaData.Id, Is.EqualTo(1));

            var existing = _provider.PluginMetaDataDataStore.GetById(1);

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

                _provider.PluginMetaDataDataStore.Create(pluginMetaData1);

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

                _provider.PluginMetaDataDataStore.Create(pluginMetaData2);
            }

            var byPackageIdList = _provider.PluginMetaDataDataStore.GetByPackageId(2);

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

            _provider.PluginMetaDataDataStore.Create(pluginMetaData);

            Assert.That(pluginMetaData.Id, Is.EqualTo(1));

            pluginMetaData.Description = "UpdatedDescription";

            _provider.PluginMetaDataDataStore.Update(pluginMetaData);

            var updated = _provider.PluginMetaDataDataStore.GetById(pluginMetaData.Id);

            Assert.That(updated.Description, Is.EqualTo("UpdatedDescription"));
        }
    }
}
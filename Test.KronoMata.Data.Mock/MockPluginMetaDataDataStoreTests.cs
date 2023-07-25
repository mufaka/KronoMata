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

            Assert.IsTrue(1 == pluginMetaData.Id);
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

            Assert.IsTrue(1 == pluginMetaData.Id);

            _provider.PluginMetaDataDataStore.Delete(pluginMetaData.Id);

            var existing = _provider.PluginMetaDataDataStore.GetById(pluginMetaData.Id);
            Assert.IsNull(existing);
        }

        [Test()]
        public void Can_GetById()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_GetByPackageId()
        {
            Assert.Fail();
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

            Assert.IsTrue(1 == pluginMetaData.Id);

            pluginMetaData.Description = "UpdatedDescription";

            _provider.PluginMetaDataDataStore.Update(pluginMetaData);

            var updated = _provider.PluginMetaDataDataStore.GetById(pluginMetaData.Id);

            Assert.That(updated.Description, Is.EqualTo("UpdatedDescription"));
        }
    }
}
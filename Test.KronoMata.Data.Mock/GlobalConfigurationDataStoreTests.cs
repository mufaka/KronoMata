using KronoMata.Data;
using KronoMata.Data.Mock;
using KronoMata.Model;

namespace Test.KronoMata.Data.Mock
{
    [TestFixture()]
    public class GlobalConfigurationDataStoreTests
    {
        private IDataStoreProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new MockDataStoreProvider();
        }

        [Test]
        public void Can_create()
        {
            var now = DateTime.Now;

            var globalConfiguration = new GlobalConfiguration()
            {
                Category = "TestCategory",
                Name = "TestName",
                Value = "TestValue",
                IsAccessibleToPlugins = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.GlobalConfigurationDataStore.Create(globalConfiguration);

            Assert.IsTrue(1 == globalConfiguration.Id);
        }

        [Test]
        public void Can_update()
        {
            var now = DateTime.Now;

            var globalConfiguration = new GlobalConfiguration()
            {
                Category = "TestCategory",
                Name = "TestName",
                Value = "TestValue",
                IsAccessibleToPlugins = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.GlobalConfigurationDataStore.Create(globalConfiguration);

            Assert.IsTrue(1 == globalConfiguration.Id);

            var existing = _provider.GlobalConfigurationDataStore.GetByCategoryAndName("TestCategory", "TestName");
            Assert.IsNotNull(existing);

            existing.Category = "UpdatedCategory";
            _provider.GlobalConfigurationDataStore.Update(existing);

            existing = _provider.GlobalConfigurationDataStore.GetByCategoryAndName("UpdatedCategory", "TestName");
            Assert.IsNotNull(existing);

            Assert.IsTrue(existing.Category == "UpdatedCategory");
        }

        [Test]
        public void Can_delete()
        {
            var now = DateTime.Now;

            var globalConfiguration = new GlobalConfiguration()
            {
                Category = "TestCategory",
                Name = "TestName",
                Value = "TestValue",
                IsAccessibleToPlugins = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.GlobalConfigurationDataStore.Create(globalConfiguration);

            Assert.IsTrue(1 == globalConfiguration.Id);

            _provider.GlobalConfigurationDataStore.Delete(globalConfiguration.Id);

            var existing = _provider.GlobalConfigurationDataStore.GetByCategoryAndName("TestCategory", "TestName");
            Assert.IsNull(existing);
        }

        [Test()]
        public void Can_GetAll()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_GetByCategory()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_GetByCategoryAndName()
        {
            Assert.Fail();
        }

    }
}

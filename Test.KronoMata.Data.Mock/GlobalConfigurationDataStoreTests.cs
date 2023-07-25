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
            var now = DateTime.Now;
            const int count = 10;

            for (int x = 0; x < count; x++)
            {
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
            }

            var all = _provider.GlobalConfigurationDataStore.GetAll();
            Assert.That(all.Count, Is.EqualTo(count));
        }

        [Test()]
        public void Can_GetByCategory()
        {
            var now = DateTime.Now;
            const int count = 10;

            for (int x = 1; x <= count; x++)
            {

                var globalConfiguration1 = new GlobalConfiguration()
                {
                    Category = "TestCategory1",
                    Name = $"TestName{x}",
                    Value = $"TestValue{x}",
                    IsAccessibleToPlugins = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                _provider.GlobalConfigurationDataStore.Create(globalConfiguration1);

                var globalConfiguration2 = new GlobalConfiguration()
                {
                    Category = "TestCategory2",
                    Name = $"TestName{x}",
                    Value = $"TestValue{x}",
                    IsAccessibleToPlugins = true,
                    InsertDate = now,
                    UpdateDate = now
                };
                
                _provider.GlobalConfigurationDataStore.Create(globalConfiguration2);
            }

            var category1 = _provider.GlobalConfigurationDataStore.GetByCategory("TestCategory1");

            Assert.That(category1.Count, Is.EqualTo(count));
        }

        [Test()]
        public void Can_GetByCategoryAndName()
        {
            var now = DateTime.Now;
            const int count = 10;

            for (int x = 0; x < count; x++)
            {
                var globalConfiguration = new GlobalConfiguration()
                {
                    Category = "TestCategory",
                    Name = $"TestName{x + 1}",
                    Value = $"TestValue{x + 1}",
                    IsAccessibleToPlugins = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                _provider.GlobalConfigurationDataStore.Create(globalConfiguration);
            }

            var one = _provider.GlobalConfigurationDataStore.GetByCategoryAndName("TestCategory", "TestName3");

            Assert.IsNotNull(one);
            Assert.That(one.Name, Is.EqualTo("TestName3"));
            Assert.That(one.Value, Is.EqualTo("TestValue3"));
        }
    }
}

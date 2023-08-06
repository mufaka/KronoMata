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

            Assert.That(globalConfiguration.Id, Is.EqualTo(1));
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

            Assert.That(globalConfiguration.Id, Is.EqualTo(1));

            var existing = _provider.GlobalConfigurationDataStore.GetById(1);
            Assert.That(existing, Is.Not.Null);

            existing.Category = "UpdatedCategory";
            _provider.GlobalConfigurationDataStore.Update(existing);

            existing = _provider.GlobalConfigurationDataStore.GetById(1);
            Assert.That(existing, Is.Not.Null);

            Assert.That(existing.Category, Is.EqualTo("UpdatedCategory"));
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

            Assert.That(globalConfiguration.Id, Is.EqualTo(1));

            _provider.GlobalConfigurationDataStore.Delete(globalConfiguration.Id);

            var existing = _provider.GlobalConfigurationDataStore.GetByCategoryAndName("TestCategory", "TestName");
            Assert.That(existing, Is.Null);
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
            Assert.That(all, Has.Count.EqualTo(count));
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

            Assert.That(category1, Has.Count.EqualTo(count));
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

            Assert.That(one, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(one.Name, Is.EqualTo("TestName3"));
                Assert.That(one.Value, Is.EqualTo("TestValue3"));
            });
        }

        [Test()]
        public void Cannot_DeleteSystemConfiguration()
        {
            var now = DateTime.Now;

            var globalConfiguration = new GlobalConfiguration()
            {
                Category = "System",
                Name = $"SomeImportantConfig",
                Value = $"SomeImportantValue",
                IsAccessibleToPlugins = true,
                IsSystemConfiguration = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.GlobalConfigurationDataStore.Create(globalConfiguration);

            Assert.That(globalConfiguration.Id, Is.EqualTo(1));

            Assert.Throws<InvalidOperationException>(() =>
            {
                _provider.GlobalConfigurationDataStore.Delete(1);
            });
        }
    }
}

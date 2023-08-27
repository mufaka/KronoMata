using KronoMata.Data;
using KronoMata.Model;
using NUnit.Framework;

namespace Test.KronoMata.Data.Base
{
    [TestFixture]
    public abstract class GlobalConfigurationDataStoreTestsBase
    {
        protected abstract IDataStoreProvider DataStoreProvider { get; }

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

            DataStoreProvider.GlobalConfigurationDataStore.Create(globalConfiguration);

            Assert.That(globalConfiguration.Id, Is.EqualTo(1));

            Assert.Throws<InvalidOperationException>(() =>
            {
                DataStoreProvider.GlobalConfigurationDataStore.Delete(1);
            });
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

            DataStoreProvider.GlobalConfigurationDataStore.Create(globalConfiguration);

            Assert.That(globalConfiguration.Id, Is.EqualTo(1));
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

            DataStoreProvider.GlobalConfigurationDataStore.Create(globalConfiguration);

            Assert.That(globalConfiguration.Id, Is.EqualTo(1));

            DataStoreProvider.GlobalConfigurationDataStore.Delete(globalConfiguration.Id);

            var existing = DataStoreProvider.GlobalConfigurationDataStore.GetByCategoryAndName("TestCategory", "TestName");
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

                DataStoreProvider.GlobalConfigurationDataStore.Create(globalConfiguration);
            }

            var all = DataStoreProvider.GlobalConfigurationDataStore.GetAll();
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

                DataStoreProvider.GlobalConfigurationDataStore.Create(globalConfiguration1);

                var globalConfiguration2 = new GlobalConfiguration()
                {
                    Category = "TestCategory2",
                    Name = $"TestName{x}",
                    Value = $"TestValue{x}",
                    IsAccessibleToPlugins = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.GlobalConfigurationDataStore.Create(globalConfiguration2);
            }

            var category1 = DataStoreProvider.GlobalConfigurationDataStore.GetByCategory("TestCategory1");

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

                DataStoreProvider.GlobalConfigurationDataStore.Create(globalConfiguration);
            }

            var one = DataStoreProvider.GlobalConfigurationDataStore.GetByCategoryAndName("TestCategory", "TestName3");

            Assert.That(one, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(one.Name, Is.EqualTo("TestName3"));
                Assert.That(one.Value, Is.EqualTo("TestValue3"));
            });
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

            DataStoreProvider.GlobalConfigurationDataStore.Create(globalConfiguration);

            Assert.That(globalConfiguration.Id, Is.EqualTo(1));

            var existing = DataStoreProvider.GlobalConfigurationDataStore.GetById(1);
            Assert.That(existing, Is.Not.Null);

            existing.Category = "UpdatedCategory";
            DataStoreProvider.GlobalConfigurationDataStore.Update(existing);

            existing = DataStoreProvider.GlobalConfigurationDataStore.GetById(1);
            Assert.That(existing, Is.Not.Null);

            Assert.That(existing.Category, Is.EqualTo("UpdatedCategory"));
        }
    }
}
using KronoMata.Data;
using KronoMata.Model;
using NUnit.Framework;

namespace Test.KronoMata.Data.Base
{
    [TestFixture]
    public abstract class HostDataStoreTestsBase
    {
        protected abstract IDataStoreProvider DataStoreProvider { get; }

        [Test()]
        public void Can_Create()
        {
            var now = DateTime.Now;

            var host = new Host()
            {
                MachineName = "TestHost",
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.HostDataStore.Create(host);

            Assert.That(host.Id, Is.EqualTo(1));
        }

        [Test()]
        public void Can_Delete()
        {
            var now = DateTime.Now;

            var host = new Host()
            {
                MachineName = "TestHost",
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.HostDataStore.Create(host);

            Assert.That(host.Id, Is.EqualTo(1));

            DataStoreProvider.HostDataStore.Delete(host.Id);

            var existing = DataStoreProvider.HostDataStore.GetByMachineName("TestHost");
            Assert.That(existing, Is.Null);
        }

        [Test()]
        public void Can_GetAll()
        {
            var now = DateTime.Now;

            for (int x = 0; x < 10; x++)
            {
                var host = new Host()
                {
                    MachineName = $"TestHost{x + 1}",
                    IsEnabled = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.HostDataStore.Create(host);
            }

            var all = DataStoreProvider.HostDataStore.GetAll();

            Assert.That(all, Has.Count.EqualTo(10));
        }

        [Test()]
        public void Can_GetByMachineName()
        {
            var now = DateTime.Now;

            var host = new Host()
            {
                MachineName = "TestHost",
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.HostDataStore.Create(host);

            Assert.That(host.Id, Is.EqualTo(1));

            var existing = DataStoreProvider.HostDataStore.GetByMachineName("TestHost");
            Assert.That(existing, Is.Not.Null);
        }

        [Test()]
        public void Can_Update()
        {
            var now = DateTime.Now;

            var host = new Host()
            {
                MachineName = "TestHost",
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.HostDataStore.Create(host);

            Assert.That(host.Id, Is.EqualTo(1));

            host.IsEnabled = false;

            DataStoreProvider.HostDataStore.Update(host);

            var existing = DataStoreProvider.HostDataStore.GetByMachineName("TestHost");

            Assert.That(existing, Is.Not.Null);
            Assert.That(existing.IsEnabled, Is.False);
        }
    }
}
using KronoMata.Data;
using KronoMata.Data.Mock;
using KronoMata.Model;

namespace Test.KronoMata.Data.Mock
{
    [TestFixture()]
    public class MockHostDataStoreTests
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

            var host = new Host()
            {
                MachineName = "TestHost",
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.HostDataStore.Create(host);

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

            _provider.HostDataStore.Create(host);

            Assert.That(host.Id, Is.EqualTo(1));

            _provider.HostDataStore.Delete(host.Id);

            var existing = _provider.HostDataStore.GetByMachineName("TestHost");
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

                _provider.HostDataStore.Create(host);
            }

            var all = _provider.HostDataStore.GetAll();

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

            _provider.HostDataStore.Create(host);

            Assert.That(host.Id, Is.EqualTo(1));

            var existing = _provider.HostDataStore.GetByMachineName("TestHost");
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

            _provider.HostDataStore.Create(host);

            Assert.That(host.Id, Is.EqualTo(1));

            host.IsEnabled = false;

            _provider.HostDataStore.Update(host);

            var existing = _provider.HostDataStore.GetByMachineName("TestHost");

            Assert.That(existing, Is.Not.Null);
            Assert.That(existing.IsEnabled, Is.False);
        }
    }
}
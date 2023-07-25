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

            Assert.IsTrue(1 == host.Id);
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

            Assert.IsTrue(1 == host.Id);

            _provider.HostDataStore.Delete(host.Id);

            var existing = _provider.HostDataStore.GetByMachineName("TestHost");
            Assert.IsNull(existing);
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

            Assert.That(all.Count, Is.EqualTo(10));
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

            Assert.IsTrue(1 == host.Id);

            var existing = _provider.HostDataStore.GetByMachineName("TestHost");
            Assert.IsNotNull(existing);
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

            Assert.IsTrue(1 == host.Id);

            host.IsEnabled = false;

            _provider.HostDataStore.Update(host);

            var existing = _provider.HostDataStore.GetByMachineName("TestHost");

            Assert.IsNotNull(existing);
            Assert.That(existing.IsEnabled, Is.False);
        }
    }
}
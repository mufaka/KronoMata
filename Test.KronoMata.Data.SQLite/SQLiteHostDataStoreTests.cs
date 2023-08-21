using KronoMata.Data;
using KronoMata.Data.SQLite;
using KronoMata.Model;

namespace Test.KronoMata.Data.SQLite
{
    [TestFixture()]
    public class SQLiteHostDataStoreTests
    {
        private IDataStoreProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new SQLiteDataStoreProvider();
            var databasePath = Path.Combine("Database", "KronoMata.db");
            SQLiteDataStoreBase.ConnectionString = $"Data Source={databasePath};Pooling=True;Cache Size=4000;Page Size=1024;FailIfMissing=True;Journal Mode=Off;";
            ClearTable();
        }

        [TearDown]
        public void ClearTable()
        {
            ((SQLiteDataStoreBase)_provider.ConfigurationValueDataStore).TruncateTable("Host");
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
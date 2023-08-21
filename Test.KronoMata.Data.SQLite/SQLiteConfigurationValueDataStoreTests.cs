using KronoMata.Data;
using KronoMata.Data.SQLite;
using KronoMata.Model;

namespace Test.KronoMata.Data.SQLite
{
    [TestFixture()]
    public class SQLiteConfigurationValueDataStoreTests
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
            ((SQLiteDataStoreBase)_provider.ConfigurationValueDataStore).TruncateTable("ConfigurationValue");
        }

        [Test]
        public void Can_create()
        {
            var now = DateTime.Now;

            var configurationValue = new ConfigurationValue()
            {
                ScheduledJobId = 1,
                PluginConfigurationId = 1,
                Value = "Dummy",
                InsertDate = now,
                UpdateDate = now
            };

            _provider.ConfigurationValueDataStore.Create(configurationValue);

            Assert.That(configurationValue.Id, Is.EqualTo(1));
        }

        [Test]
        public void Can_update()
        {
            var now = DateTime.Now;

            var configurationValue = new ConfigurationValue()
            {
                ScheduledJobId = 1,
                PluginConfigurationId = 1,
                Value = "Dummy",
                InsertDate = now,
                UpdateDate = now
            };

            _provider.ConfigurationValueDataStore.Create(configurationValue);

            var existing = _provider.ConfigurationValueDataStore.GetByScheduledJob(1);

            Assert.That(existing, Has.Count.EqualTo(1));

            var tomorrow = now.AddDays(1);

            configurationValue.Value = "Dummy Updated";
            configurationValue.UpdateDate = tomorrow;

            _provider.ConfigurationValueDataStore.Update(configurationValue);

            existing = _provider.ConfigurationValueDataStore.GetByScheduledJob(1);

            Assert.That(existing, Has.Count.EqualTo(1));
            Assert.That(existing[0].Value, Is.EqualTo("Dummy Updated"));
            Assert.That(existing[0].UpdateDate, Is.EqualTo(tomorrow));
        }

        [Test]
        public void Can_delete()
        {
            var now = DateTime.Now;

            var configurationValue = new ConfigurationValue()
            {
                ScheduledJobId = 1,
                PluginConfigurationId = 1,
                Value = "Dummy",
                InsertDate = now,
                UpdateDate = now
            };

            _provider.ConfigurationValueDataStore.Create(configurationValue);
            var existing = _provider.ConfigurationValueDataStore.GetByScheduledJob(1);

            Assert.That(existing, Has.Count.EqualTo(1));

            _provider.ConfigurationValueDataStore.Delete(configurationValue.Id);
            existing = _provider.ConfigurationValueDataStore.GetByScheduledJob(1);

            Assert.That(existing, Is.Empty);
        }

        [Test]
        public void Can_get_by_scheduled_job()
        {
            var now = DateTime.Now;
            const int count = 5;

            for (int x = 0; x < count; x++)
            {
                var configurationValue1 = new ConfigurationValue()
                {
                    ScheduledJobId = 1,
                    PluginConfigurationId = x,
                    Value = $"Dummy {x}",
                    InsertDate = now,
                    UpdateDate = now
                };

                _provider.ConfigurationValueDataStore.Create(configurationValue1);

                var configurationValue2 = new ConfigurationValue()
                {
                    ScheduledJobId = 2,
                    PluginConfigurationId = x,
                    Value = $"Dummy {x}",
                    InsertDate = now,
                    UpdateDate = now
                };

                _provider.ConfigurationValueDataStore.Create(configurationValue2);

            }

            var byScheduledJob = _provider.ConfigurationValueDataStore.GetByScheduledJob(1);

            Assert.That(byScheduledJob, Has.Count.EqualTo(count));

            foreach (var configurationValue in byScheduledJob)
            {
                Assert.That(configurationValue.ScheduledJobId, Is.EqualTo(1));
            }
        }
    }
}
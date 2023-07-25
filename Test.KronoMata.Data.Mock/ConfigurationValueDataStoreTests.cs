using KronoMata.Data;
using KronoMata.Data.Mock;
using KronoMata.Model;

namespace Test.KronoMata.Data.Mock
{
    [TestFixture()]
    public class ConfigurationValueDataStoreTests
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

            var configurationValue = new ConfigurationValue()
            {
                ScheduledJobId = 1,
                PluginConfigurationId = 1,
                Value = "Dummy",
                InsertDate = now,
                UpdateDate = now
            };

            _provider.ConfigurationValueDataStore.Create(configurationValue);

            Assert.IsTrue(1 == configurationValue.Id);
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

            Assert.IsTrue(1 == existing.Count);

            configurationValue.Value = "Dummy Updated";

            _provider.ConfigurationValueDataStore.Update(configurationValue);

            existing = _provider.ConfigurationValueDataStore.GetByScheduledJob(1);

            Assert.IsTrue(1 == existing.Count);
            Assert.IsTrue("Dummy Updated" == existing[0].Value);
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

            Assert.IsTrue(1 == existing.Count);

            _provider.ConfigurationValueDataStore.Delete(configurationValue.Id);
            existing = _provider.ConfigurationValueDataStore.GetByScheduledJob(1);

            Assert.IsTrue(0 == existing.Count);
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

            Assert.IsTrue(byScheduledJob.Count == count);

            foreach (var configurationValue in byScheduledJob)
            {
                Assert.IsTrue(1 == configurationValue.ScheduledJobId);
            }
        }
    }
}
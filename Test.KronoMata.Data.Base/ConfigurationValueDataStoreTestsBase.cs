using KronoMata.Data;
using KronoMata.Model;
using NUnit.Framework;

namespace Test.KronoMata.Data.Base
{
    [TestFixture]
    public abstract class ConfigurationValueDataStoreTestsBase
    {
        protected abstract IDataStoreProvider DataStoreProvider { get; }

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

            DataStoreProvider.ConfigurationValueDataStore.Create(configurationValue);

            Assert.That(configurationValue.Id, Is.EqualTo(1));
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

            DataStoreProvider.ConfigurationValueDataStore.Create(configurationValue);
            var existing = DataStoreProvider.ConfigurationValueDataStore.GetByScheduledJob(1);

            Assert.That(existing, Has.Count.EqualTo(1));

            DataStoreProvider.ConfigurationValueDataStore.Delete(configurationValue.Id);
            existing = DataStoreProvider.ConfigurationValueDataStore.GetByScheduledJob(1);

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

                DataStoreProvider.ConfigurationValueDataStore.Create(configurationValue1);

                var configurationValue2 = new ConfigurationValue()
                {
                    ScheduledJobId = 2,
                    PluginConfigurationId = x,
                    Value = $"Dummy {x}",
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.ConfigurationValueDataStore.Create(configurationValue2);

            }

            var byScheduledJob = DataStoreProvider.ConfigurationValueDataStore.GetByScheduledJob(1);

            Assert.That(byScheduledJob, Has.Count.EqualTo(count));

            foreach (var configurationValue in byScheduledJob)
            {
                Assert.That(configurationValue.ScheduledJobId, Is.EqualTo(1));
            }
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

            DataStoreProvider.ConfigurationValueDataStore.Create(configurationValue);

            var existing = DataStoreProvider.ConfigurationValueDataStore.GetByScheduledJob(1);

            Assert.That(existing, Has.Count.EqualTo(1));

            var tomorrow = now.AddDays(1);

            configurationValue.Value = "Dummy Updated";
            configurationValue.UpdateDate = tomorrow;

            DataStoreProvider.ConfigurationValueDataStore.Update(configurationValue);

            existing = DataStoreProvider.ConfigurationValueDataStore.GetByScheduledJob(1);

            Assert.That(existing, Has.Count.EqualTo(1));
            Assert.That(existing[0].Value, Is.EqualTo("Dummy Updated"));
            Assert.That(existing[0].UpdateDate, Is.EqualTo(tomorrow));
        }
    }
}
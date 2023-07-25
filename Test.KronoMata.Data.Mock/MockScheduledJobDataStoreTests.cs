using KronoMata.Data;
using KronoMata.Data.Mock;
using KronoMata.Model;

namespace Test.KronoMata.Data.Mock
{
    [TestFixture()]
    public class MockScheduledJobDataStoreTests
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

            var scheduledJob = new ScheduledJob()
            {
                PluginMetaDataId = 1,
                HostId = 1,
                Name = "Name",
                Description = "Description",
                Interval = ScheduleInterval.Week,
                Step = 2,
                StartTime = now,
                EndTime = now,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.IsTrue(1 == scheduledJob.Id);
        }

        [Test()]
        public void Can_Delete()
        {
            var now = DateTime.Now;

            var scheduledJob = new ScheduledJob()
            {
                PluginMetaDataId = 1,
                HostId = 1,
                Name = "Name",
                Description = "Description",
                Interval = ScheduleInterval.Week,
                Step = 2,
                StartTime = now,
                EndTime = now,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.IsTrue(1 == scheduledJob.Id);

            _provider.ScheduledJobDataStore.Delete(scheduledJob.Id);

            var existing = _provider.ScheduledJobDataStore.GetById(scheduledJob.Id);
            Assert.IsNull(existing);
        }

        [Test()]
        public void Can_GetAll()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_GetByHost()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_GetById()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_GetByPluginMetaData()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_Update()
        {
            var now = DateTime.Now;

            var scheduledJob = new ScheduledJob()
            {
                PluginMetaDataId = 1,
                HostId = 1,
                Name = "Name",
                Description = "Description",
                Interval = ScheduleInterval.Week,
                Step = 2,
                StartTime = now,
                EndTime = now,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.IsTrue(1 == scheduledJob.Id);

            scheduledJob.Description = "UpdatedDescription";

            _provider.ScheduledJobDataStore.Update(scheduledJob);

            var updated = _provider.ScheduledJobDataStore.GetById(scheduledJob.Id);

            Assert.That(updated.Description, Is.EqualTo("UpdatedDescription"));
        }
    }
}
using KronoMata.Data;
using KronoMata.Model;
using NUnit.Framework;

namespace Test.KronoMata.Data.Base
{
    [TestFixture]
    public abstract class ScheduledJobDataStoreTestsBase
    {
        protected abstract IDataStoreProvider DataStoreProvider { get; }

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
                Frequency = ScheduleFrequency.Week,
                Interval = 2,
                StartTime = now,
                EndTime = now,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.That(scheduledJob.Id, Is.EqualTo(1));
        }

        [Test()]
        public void Can_Create_With_Null_Host()
        {
            var now = DateTime.Now;

            var scheduledJob = new ScheduledJob()
            {
                PluginMetaDataId = 1,
                HostId = null,
                Name = "Name",
                Description = "Description",
                Frequency = ScheduleFrequency.Week,
                Interval = 2,
                StartTime = now,
                EndTime = now,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.That(scheduledJob.Id, Is.EqualTo(1));
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
                Frequency = ScheduleFrequency.Week,
                Interval = 2,
                StartTime = now,
                EndTime = now,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.That(scheduledJob.Id, Is.EqualTo(1));

            DataStoreProvider.ScheduledJobDataStore.Delete(scheduledJob.Id);

            var existing = DataStoreProvider.ScheduledJobDataStore.GetById(scheduledJob.Id);
            Assert.That(existing, Is.Null);
        }

        [Test()]
        public void Can_GetAll()
        {
            var now = DateTime.Now;

            for (int x = 1; x <= 10; x++)
            {
                var scheduledJob = new ScheduledJob()
                {
                    PluginMetaDataId = 1,
                    HostId = 1,
                    Name = "Name",
                    Description = "Description",
                    Frequency = ScheduleFrequency.Week,
                    Interval = 2,
                    StartTime = now,
                    EndTime = now,
                    IsEnabled = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob);
            }

            var all = DataStoreProvider.ScheduledJobDataStore.GetAll();
            Assert.That(all, Has.Count.EqualTo(10));
        }

        [Test()]
        public void Can_GetByHost()
        {
            var now = DateTime.Now;

            for (int x = 1; x <= 10; x++)
            {
                var scheduledJob1 = new ScheduledJob()
                {
                    PluginMetaDataId = 1,
                    HostId = 1,
                    Name = "Name",
                    Description = "Description",
                    Frequency = ScheduleFrequency.Week,
                    Interval = 2,
                    StartTime = now,
                    EndTime = now,
                    IsEnabled = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob1);

                var scheduledJob2 = new ScheduledJob()
                {
                    PluginMetaDataId = 1,
                    HostId = 2,
                    Name = "Name",
                    Description = "Description",
                    Frequency = ScheduleFrequency.Week,
                    Interval = 2,
                    StartTime = now,
                    EndTime = now,
                    IsEnabled = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob2);
            }

            var byHostList = DataStoreProvider.ScheduledJobDataStore.GetByHost(2);

            Assert.That(byHostList, Has.Count.EqualTo(10));
        }

        [Test()]
        public void Can_GetById()
        {
            var now = DateTime.Now;

            var scheduledJob = new ScheduledJob()
            {
                PluginMetaDataId = 1,
                HostId = 1,
                Name = "Name",
                Description = "Description",
                Frequency = ScheduleFrequency.Week,
                Interval = 2,
                StartTime = now,
                EndTime = now,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.That(scheduledJob.Id, Is.EqualTo(1));

            var existing = DataStoreProvider.ScheduledJobDataStore.GetById(1);

            Assert.That(existing, Is.Not.Null);
            Assert.That(existing.Id, Is.EqualTo(1));
        }

        [Test()]
        public void Can_GetByPluginMetaData()
        {
            var now = DateTime.Now;

            for (int x = 1; x <= 10; x++)
            {
                var scheduledJob1 = new ScheduledJob()
                {
                    PluginMetaDataId = 1,
                    HostId = 1,
                    Name = "Name",
                    Description = "Description",
                    Frequency = ScheduleFrequency.Week,
                    Interval = 2,
                    StartTime = now,
                    EndTime = now,
                    IsEnabled = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob1);

                var scheduledJob2 = new ScheduledJob()
                {
                    PluginMetaDataId = 2,
                    HostId = 1,
                    Name = "Name",
                    Description = "Description",
                    Frequency = ScheduleFrequency.Week,
                    Interval = 2,
                    StartTime = now,
                    EndTime = now,
                    IsEnabled = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob2);
            }

            var byPluginMetaDataList = DataStoreProvider.ScheduledJobDataStore.GetByPluginMetaData(2);

            Assert.That(byPluginMetaDataList, Has.Count.EqualTo(10));
        }

        [Test()]
        public void Can_GetBy_Null_Host()
        {
            var now = DateTime.Now;

            for (int x = 1; x <= 10; x++)
            {
                var scheduledJob1 = new ScheduledJob()
                {
                    PluginMetaDataId = 1,
                    HostId = null,
                    Name = "Name",
                    Description = "Description",
                    Frequency = ScheduleFrequency.Week,
                    Interval = 2,
                    StartTime = now,
                    EndTime = now,
                    IsEnabled = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob1);

                var scheduledJob2 = new ScheduledJob()
                {
                    PluginMetaDataId = 1,
                    HostId = 2,
                    Name = "Name",
                    Description = "Description",
                    Frequency = ScheduleFrequency.Week,
                    Interval = 2,
                    StartTime = now,
                    EndTime = now,
                    IsEnabled = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob2);
            }

            var byHostList = DataStoreProvider.ScheduledJobDataStore.GetByHost(2);

            // we should get all jobs. 10 for the host specifically, 10 that have a null host id.
            Assert.That(byHostList, Has.Count.EqualTo(20));
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
                Frequency = ScheduleFrequency.Week,
                Interval = 2,
                StartTime = now,
                EndTime = now,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.That(scheduledJob.Id, Is.EqualTo(1));

            scheduledJob.Description = "UpdatedDescription";

            DataStoreProvider.ScheduledJobDataStore.Update(scheduledJob);

            var updated = DataStoreProvider.ScheduledJobDataStore.GetById(scheduledJob.Id);

            Assert.That(updated.Description, Is.EqualTo("UpdatedDescription"));
        }

        [Test()]
        public void Can_Update_With_Null_Host()
        {
            var now = DateTime.Now;

            var scheduledJob = new ScheduledJob()
            {
                PluginMetaDataId = 1,
                HostId = null,
                Name = "Name",
                Description = "Description",
                Frequency = ScheduleFrequency.Week,
                Interval = 2,
                StartTime = now,
                EndTime = now,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.That(scheduledJob.Id, Is.EqualTo(1));

            scheduledJob.Description = "UpdatedDescription";

            DataStoreProvider.ScheduledJobDataStore.Update(scheduledJob);

            var updated = DataStoreProvider.ScheduledJobDataStore.GetById(scheduledJob.Id);

            Assert.That(updated.Description, Is.EqualTo("UpdatedDescription"));
        }

        [Test()]
        public void Will_Get_Only_Null_And_Matching_Host()
        {
            var now = DateTime.Now;

            for (int x = 1; x <= 10; x++)
            {
                var scheduledJob1 = new ScheduledJob()
                {
                    PluginMetaDataId = 1,
                    HostId = null,
                    Name = "Name",
                    Description = "Description",
                    Frequency = ScheduleFrequency.Week,
                    Interval = 2,
                    StartTime = now,
                    EndTime = now,
                    IsEnabled = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob1);

                var scheduledJob2 = new ScheduledJob()
                {
                    PluginMetaDataId = 1,
                    HostId = 2,
                    Name = "Name",
                    Description = "Description",
                    Frequency = ScheduleFrequency.Week,
                    Interval = 2,
                    StartTime = now,
                    EndTime = now,
                    IsEnabled = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob2);

                var scheduledJob3 = new ScheduledJob()
                {
                    PluginMetaDataId = 1,
                    HostId = 3,
                    Name = "Name",
                    Description = "Description",
                    Frequency = ScheduleFrequency.Week,
                    Interval = 2,
                    StartTime = now,
                    EndTime = now,
                    IsEnabled = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob3);
            }

            var byHostList = DataStoreProvider.ScheduledJobDataStore.GetByHost(2);

            // we should get all jobs. 10 for the host specifically, 10 that have a null host id.
            Assert.That(byHostList, Has.Count.EqualTo(20));
        }
    }
}
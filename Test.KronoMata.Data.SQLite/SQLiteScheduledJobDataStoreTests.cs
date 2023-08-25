using KronoMata.Data;
using KronoMata.Data.SQLite;
using KronoMata.Model;

namespace Test.KronoMata.Data.SQLite
{
    [TestFixture()]
    public class SQLiteScheduledJobDataStoreTests
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
            ((SQLiteDataStoreBase)_provider.ConfigurationValueDataStore).TruncateTable("ScheduledJob");
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
                Frequency = ScheduleFrequency.Week,
                Interval = 2,
                StartTime = now,
                EndTime = now,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            _provider.ScheduledJobDataStore.Create(scheduledJob);

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

            _provider.ScheduledJobDataStore.Create(scheduledJob);

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

            _provider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.That(scheduledJob.Id, Is.EqualTo(1));

            _provider.ScheduledJobDataStore.Delete(scheduledJob.Id);

            var existing = _provider.ScheduledJobDataStore.GetById(scheduledJob.Id);
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

                _provider.ScheduledJobDataStore.Create(scheduledJob);
            }

            var all = _provider.ScheduledJobDataStore.GetAll();
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

                _provider.ScheduledJobDataStore.Create(scheduledJob1);

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

                _provider.ScheduledJobDataStore.Create(scheduledJob2);
            }

            var byHostList = _provider.ScheduledJobDataStore.GetByHost(2);

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

            _provider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.That(scheduledJob.Id, Is.EqualTo(1));

            var existing = _provider.ScheduledJobDataStore.GetById(1);

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

                _provider.ScheduledJobDataStore.Create(scheduledJob1);

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

                _provider.ScheduledJobDataStore.Create(scheduledJob2);
            }

            var byPluginMetaDataList = _provider.ScheduledJobDataStore.GetByPluginMetaData(2);

            Assert.That(byPluginMetaDataList, Has.Count.EqualTo(10));
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

            _provider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.That(scheduledJob.Id, Is.EqualTo(1));

            scheduledJob.Description = "UpdatedDescription";

            _provider.ScheduledJobDataStore.Update(scheduledJob);

            var updated = _provider.ScheduledJobDataStore.GetById(scheduledJob.Id);

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

            _provider.ScheduledJobDataStore.Create(scheduledJob);

            Assert.That(scheduledJob.Id, Is.EqualTo(1));

            scheduledJob.Description = "UpdatedDescription";

            _provider.ScheduledJobDataStore.Update(scheduledJob);

            var updated = _provider.ScheduledJobDataStore.GetById(scheduledJob.Id);

            Assert.That(updated.Description, Is.EqualTo("UpdatedDescription"));
        }
    }
}
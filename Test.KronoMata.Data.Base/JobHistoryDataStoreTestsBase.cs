using KronoMata.Data;
using KronoMata.Model;
using NUnit.Framework;

namespace Test.KronoMata.Data.Base
{
    [TestFixture]
    public abstract class JobHistoryDataStoreTestsBase
    {
        protected abstract IDataStoreProvider DataStoreProvider { get; }

        [Test()]
        public void Can_Create()
        {
            var now = DateTime.Now;

            var jobHistory = new JobHistory()
            {
                ScheduledJobId = 1,
                HostId = 1,
                Status = ScheduledJobStatus.Success,
                Message = "TestMessage",
                Detail = "TestDetail",
                RunTime = now
            };

            DataStoreProvider.JobHistoryDataStore.Create(jobHistory);

            Assert.That(jobHistory.Id, Is.EqualTo(1));
        }

        [Test()]
        public void Can_Delete()
        {
            var now = DateTime.Now;

            var jobHistory = new JobHistory()
            {
                ScheduledJobId = 1,
                HostId = 1,
                Status = ScheduledJobStatus.Success,
                Message = "TestMessage",
                Detail = "TestDetail",
                RunTime = now
            };

            DataStoreProvider.JobHistoryDataStore.Create(jobHistory);

            Assert.That(jobHistory.Id, Is.EqualTo(1));

            DataStoreProvider.JobHistoryDataStore.Delete(jobHistory.Id);

            var existing = DataStoreProvider.JobHistoryDataStore.GetAll();

            Assert.That(existing, Is.Empty);
        }

        [Test()]
        public void Can_GetAll()
        {
            var now = DateTime.Now;

            for (int x = 0; x < 10; x++)
            {
                var jobHistory = new JobHistory()
                {
                    ScheduledJobId = 1,
                    HostId = 1,
                    Status = ScheduledJobStatus.Success,
                    Message = $"TestMessage{x + 1}",
                    Detail = $"TestDetail{x + 1}",
                    RunTime = now
                };

                DataStoreProvider.JobHistoryDataStore.Create(jobHistory);
            }

            var all = DataStoreProvider.JobHistoryDataStore.GetAll();

            Assert.That(all, Has.Count.EqualTo(10));
        }

        [Test()]
        public void Can_GetByHost()
        {
            var now = DateTime.Now;

            for (int x = 1; x <= 10; x++)
            {
                var jobHistory1 = new JobHistory()
                {
                    ScheduledJobId = x,
                    HostId = 1,
                    Status = ScheduledJobStatus.Success,
                    Message = $"TestMessage{x + 1}",
                    Detail = $"TestDetail{x + 1}",
                    RunTime = now
                };

                DataStoreProvider.JobHistoryDataStore.Create(jobHistory1);

                var jobHistory2 = new JobHistory()
                {
                    ScheduledJobId = x,
                    HostId = 2,
                    Status = ScheduledJobStatus.Success,
                    Message = $"TestMessage{x + 1}",
                    Detail = $"TestDetail{x + 1}",
                    RunTime = now
                };

                DataStoreProvider.JobHistoryDataStore.Create(jobHistory2);
            }

            var byHostList = DataStoreProvider.JobHistoryDataStore.GetByHost(2, 0, 20).List;

            Assert.That(byHostList, Has.Count.EqualTo(10));
        }

        [Test()]
        public void Can_GetByScheduledJob()
        {
            var now = DateTime.Now;

            for (int x = 1; x <= 10; x++)
            {
                var jobHistory1 = new JobHistory()
                {
                    ScheduledJobId = x,
                    HostId = 1,
                    Status = ScheduledJobStatus.Success,
                    Message = $"TestMessage{x + 1}",
                    Detail = $"TestDetail{x + 1}",
                    RunTime = now
                };

                DataStoreProvider.JobHistoryDataStore.Create(jobHistory1);

                var jobHistory2 = new JobHistory()
                {
                    ScheduledJobId = x,
                    HostId = 1,
                    Status = ScheduledJobStatus.Success,
                    Message = $"TestMessage{x + 1}",
                    Detail = $"TestDetail{x + 1}",
                    RunTime = now
                };

                DataStoreProvider.JobHistoryDataStore.Create(jobHistory2);
            }

            var byJobList = DataStoreProvider.JobHistoryDataStore.GetByScheduledJob(2, 0, 10).List;

            Assert.That(byJobList, Has.Count.EqualTo(2));
        }

        [Test()]
        public void Can_GetPaged()
        {
            var now = DateTime.Now;

            for (int x = 0; x < 20; x++)
            {
                var jobHistory = new JobHistory()
                {
                    ScheduledJobId = 1,
                    HostId = 1,
                    Status = ScheduledJobStatus.Success,
                    Message = $"TestMessage{x + 1}",
                    Detail = $"TestDetail{x + 1}",
                    RunTime = now
                };

                DataStoreProvider.JobHistoryDataStore.Create(jobHistory);
            }

            var paged = DataStoreProvider.JobHistoryDataStore.GetAllPaged(0, 10);

            Assert.That(paged.TotalRecords, Is.EqualTo(20));
            Assert.That(paged.List, Has.Count.EqualTo(10));
        }

        [Test()]
        public void Can_GetTop()
        {
            var now = DateTime.Now;

            for (int x = 1; x <= 10; x++)
            {
                var jobHistory1 = new JobHistory()
                {
                    ScheduledJobId = x,
                    HostId = 1,
                    Status = ScheduledJobStatus.Success,
                    Message = $"TestMessage{x + 1}",
                    Detail = $"TestDetail{x + 1}",
                    RunTime = now
                };

                DataStoreProvider.JobHistoryDataStore.Create(jobHistory1);

                var jobHistory2 = new JobHistory()
                {
                    ScheduledJobId = x,
                    HostId = 1,
                    Status = ScheduledJobStatus.Success,
                    Message = $"TestMessage{x + 1}",
                    Detail = $"TestDetail{x + 1}",
                    RunTime = now
                };

                DataStoreProvider.JobHistoryDataStore.Create(jobHistory2);
            }

            var byJobList = DataStoreProvider.JobHistoryDataStore.GetTop(5);

            Assert.That(byJobList, Has.Count.EqualTo(5));
        }
    }
}
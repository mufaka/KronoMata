using KronoMata.Data;
using KronoMata.Data.Mock;
using KronoMata.Model;

namespace Test.KronoMata.Data.Mock
{
    [TestFixture()]
    public class MockJobHistoryDataStoreTests
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

            var jobHistory = new JobHistory()
            {
                ScheduledJobId = 1,
                HostId = 1,
                Status = ScheduledJobStatus.Success,
                Message = "TestMessage",
                Detail = "TestDetail",
                RunTime = now
            };

            _provider.JobHistoryDataStore.Create(jobHistory);

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

            _provider.JobHistoryDataStore.Create(jobHistory);

            Assert.That(jobHistory.Id, Is.EqualTo(1));

            _provider.JobHistoryDataStore.Delete(jobHistory.Id);

            var existing = _provider.JobHistoryDataStore.GetAll();
            
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

                _provider.JobHistoryDataStore.Create(jobHistory);
            }

            var all = _provider.JobHistoryDataStore.GetAll();

            Assert.That(all, Has.Count.EqualTo(10));
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

                _provider.JobHistoryDataStore.Create(jobHistory1);

                var jobHistory2 = new JobHistory()
                {
                    ScheduledJobId = x,
                    HostId = 1,
                    Status = ScheduledJobStatus.Success,
                    Message = $"TestMessage{x + 1}",
                    Detail = $"TestDetail{x + 1}",
                    RunTime = now
                };

                _provider.JobHistoryDataStore.Create(jobHistory2);
            }

            var byJobList = _provider.JobHistoryDataStore.GetByScheduledJob(2, 0, 10).List;

            Assert.That(byJobList, Has.Count.EqualTo(2));
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

                _provider.JobHistoryDataStore.Create(jobHistory1);

                var jobHistory2 = new JobHistory()
                {
                    ScheduledJobId = x,
                    HostId = 2,
                    Status = ScheduledJobStatus.Success,
                    Message = $"TestMessage{x + 1}",
                    Detail = $"TestDetail{x + 1}",
                    RunTime = now
                };

                _provider.JobHistoryDataStore.Create(jobHistory2);
            }

            var byHostList = _provider.JobHistoryDataStore.GetByHost(2, 0, 20).List;

            Assert.That(byHostList, Has.Count.EqualTo(10));
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

                _provider.JobHistoryDataStore.Create(jobHistory1);

                var jobHistory2 = new JobHistory()
                {
                    ScheduledJobId = x,
                    HostId = 1,
                    Status = ScheduledJobStatus.Success,
                    Message = $"TestMessage{x + 1}",
                    Detail = $"TestDetail{x + 1}",
                    RunTime = now
                };

                _provider.JobHistoryDataStore.Create(jobHistory2);
            }

            var byJobList = _provider.JobHistoryDataStore.GetTop(5);

            Assert.That(byJobList, Has.Count.EqualTo(5));
        }
    }
}
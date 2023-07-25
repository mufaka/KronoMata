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

            Assert.IsTrue(1 == jobHistory.Id);
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

            Assert.IsTrue(1 == jobHistory.Id);

            _provider.JobHistoryDataStore.Delete(jobHistory.Id);

            var existing = _provider.JobHistoryDataStore.GetByScheduledJob(1);
            
            Assert.That(existing.Count, Is.EqualTo(0));
        }

        [Test()]
        public void Can_GetAll()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_GetByScheduledJob()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_GetTop()
        {
            Assert.Fail();
        }
    }
}
using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockJobHistoryDataStore : BaseMockDataStore, IJobHistoryDataStore
    {
        public MockJobHistoryDataStore(MockDataStoreProvider dataProvider) : base(dataProvider) { }

        private readonly List<JobHistory> _jobHistories = new();

        public JobHistory Create(JobHistory jobHistory)
        {
            jobHistory.Id = _jobHistories.Count == 0
                ? 1
                : _jobHistories[^1].Id + 1;

            _jobHistories.Add(jobHistory);

            return jobHistory;
        }

        public void Delete(int id)
        {
            //_jobHistories.RemoveAll(j => j.Id == id);

            var existingIndex = _jobHistories.FindIndex(g => g.Id == id);

            if (existingIndex >= 0)
            {
                _jobHistories.RemoveAt(existingIndex);
            }
        }

        public List<JobHistory> GetAll()
        {
            return _jobHistories;
        }

        public PagedList<JobHistory> GetAllPaged(int pageIndex, int pageSize)
        {
            return new PagedList<JobHistory>()
            {
                TotalRecords = _jobHistories.Count,
                List = _jobHistories.Skip(pageIndex * pageSize).Take(pageSize).ToList()
            };
        }

        public List<JobHistory> GetByScheduledJob(int scheduledJobId)
        {
            return _jobHistories.Where(j => j.ScheduledJobId == scheduledJobId).ToList();
        }

        public List<JobHistory> GetTop(int howMany)
        {
            return Enumerable.Reverse(_jobHistories).Take(howMany).ToList();
        }

        public List<JobHistory> GetLastByDate(DateTime startDate)
        {
            return _jobHistories.Where(h => h.RunTime > startDate).OrderByDescending(h => h.RunTime).ToList();
        }

        public PagedList<JobHistory> GetLastByDatePaged(DateTime startDate, int pageIndex, int pageSize)
        {
            var all = _jobHistories.Where(h => h.RunTime > startDate).OrderByDescending(h => h.RunTime).ToList();

            return new PagedList<JobHistory>()
            {
                TotalRecords = all.Count,
                List = all.Skip(pageIndex * pageSize).Take(pageSize).ToList()
            };
        }
    }
}

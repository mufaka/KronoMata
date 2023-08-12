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
            return _jobHistories.OrderByDescending(h => h.RunTime).ToList();
        }

        public PagedList<JobHistory> GetAllPaged(int pageIndex, int pageSize)
        {
            return new PagedList<JobHistory>()
            {
                TotalRecords = _jobHistories.Count,
                List = _jobHistories
                    .OrderByDescending(h => h.RunTime)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize).ToList()
            };
        }

        public PagedList<JobHistory> GetFilteredPaged(int pageIndex, int pageSize, int status = -1, int scheduledJobId = -1, int hostId = -1)
        {
            var filtered = _jobHistories.Where(h =>
                (status == -1 || h.Status == (ScheduledJobStatus)status) &&
                (scheduledJobId == -1 || h.ScheduledJobId == scheduledJobId) &&
                (hostId == -1 || h.HostId == hostId))
                .OrderByDescending(h => h.RunTime).ToList();

            return new PagedList<JobHistory>()
            {
                TotalRecords = filtered.Count,
                List = filtered.Skip(pageIndex * pageSize)
                .Take(pageSize).ToList()

            };
        }
        public PagedList<JobHistory> GetByScheduledJob(int scheduledJobId, int pageIndex, int pageSize)
        {
            return GetFilteredPaged(pageIndex, pageSize, -1, scheduledJobId, -1);
        }

        public PagedList<JobHistory> GetByHost(int hostId, int pageIndex, int pageSize)
        {
            return GetFilteredPaged(pageIndex, pageSize, -1, -1, hostId);
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

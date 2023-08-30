using KronoMata.Model;
using KronoMata.Model.Stats;

namespace KronoMata.Data.Mock
{
    public class MockJobHistoryDataStore : BaseMockDataStore, IJobHistoryDataStore
    {
        public MockJobHistoryDataStore(MockDataStoreProvider dataProvider) : base(dataProvider) { }

        private List<JobHistory> _jobHistories = new();

        public void Initialize(List<JobHistory> jobHistories) { _jobHistories = jobHistories; }

        public JobHistory Create(JobHistory jobHistory)
        {
            if (jobHistory.Id <= 0)
            {
                jobHistory.Id = _jobHistories.Count == 0
                    ? 1
                    : _jobHistories[^1].Id + 1;
            }

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

        public TableStat GetTableStat()
        {
            var tableStat = new TableStat();

            tableStat.TableName = "JobHistory";

            // ensure that the order is what we expect it to be
            var ascendingOrderHistory = _jobHistories.OrderBy(h => h.RunTime).ToList();

            tableStat.RowCount = ascendingOrderHistory.Count;
            tableStat.OldestRecord = ascendingOrderHistory.Count == 0 ? null : ascendingOrderHistory[0].RunTime;
            tableStat.NewestRecord = ascendingOrderHistory.Count == 0 ? null : ascendingOrderHistory[^1].RunTime;

            return tableStat;
        }

        public int Expire(int maxDays, int maxRecords)
        {
            int affectedRows = 0;
            int originalCount = _jobHistories.Count;
            var oldestDate = DateTime.Now.Date.AddDays(-maxDays);

            // ensure that the order is what we expect it to be
            var ascendingOrderHistory = _jobHistories.OrderBy(h => h.RunTime).ToList();

            // remove all but last maxRecords count
            var lastXRecords = ascendingOrderHistory.TakeLast(maxRecords).ToList();

            // remove any that are older than the calculated maxdays date
            lastXRecords.RemoveAll(h => h.RunTime < oldestDate);

            // get the delta between original and new
            affectedRows = originalCount - lastXRecords.Count;

            _jobHistories = lastXRecords;

            return affectedRows;
        }
    }
}

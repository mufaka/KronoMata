using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockJobHistoryDataStore : IJobHistoryDataStore
    {
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
            _jobHistories.RemoveAll(j => j.Id == id);
        }

        public List<JobHistory> GetAll()
        {
            return _jobHistories;
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
    }
}

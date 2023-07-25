using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockJobHistoryDataStore : IJobHistoryDataStore
    {
        public JobHistory Create(JobHistory jobHistory)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<JobHistory> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<JobHistory> GetByScheduledJob(int scheduledJobId)
        {
            throw new NotImplementedException();
        }

        public List<JobHistory> GetTop(int howMany)
        {
            throw new NotImplementedException();
        }
    }
}

using KronoMata.Model;

namespace KronoMata.Data.SQLite
{
    public class SQLiteJobHistoryDataStore : SQLiteDataStoreBase, IJobHistoryDataStore
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

        public PagedList<JobHistory> GetAllPaged(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public PagedList<JobHistory> GetByHost(int hostId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public PagedList<JobHistory> GetByScheduledJob(int scheduledJobId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public PagedList<JobHistory> GetFilteredPaged(int pageIndex, int pageSize, int status, int scheduledJobId, int hostId)
        {
            throw new NotImplementedException();
        }

        public List<JobHistory> GetLastByDate(DateTime startDate)
        {
            throw new NotImplementedException();
        }

        public PagedList<JobHistory> GetLastByDatePaged(DateTime startDate, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public List<JobHistory> GetTop(int howMany)
        {
            throw new NotImplementedException();
        }
    }
}

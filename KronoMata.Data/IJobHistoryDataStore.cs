using KronoMata.Model;

namespace KronoMata.Data
{
    /// <summary>
    /// Defines a DataStore for persisting JobHistory.
    /// </summary>
    public interface IJobHistoryDataStore
    {
        JobHistory Create(JobHistory jobHistory);
        void Delete(int id);
        List<JobHistory> GetAll();
        PagedList<JobHistory> GetAllPaged(int pageIndex, int pageSize);
        PagedList<JobHistory> GetFilteredPaged(int pageIndex, int pageSize, int status, int scheduledJobId, int hostId);
        List<JobHistory> GetTop(int howMany);
        List<JobHistory> GetByScheduledJob(int scheduledJobId);
        List<JobHistory> GetLastByDate(DateTime startDate);
        PagedList<JobHistory> GetLastByDatePaged(DateTime startDate, int pageIndex, int pageSize);
    }
}

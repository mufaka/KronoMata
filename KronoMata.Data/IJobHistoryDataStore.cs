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
        List<JobHistory> GetTop(int howMany);
        List<JobHistory> GetByScheduledJob(int scheduledJobId);

        List<JobHistory> GetLastByDate(DateTime startDate);
    }
}

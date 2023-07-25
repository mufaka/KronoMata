using KronoMata.Model;

namespace KronoMata.Data
{
    public interface IJobHistoryDataStore
    {
        JobHistory Create(JobHistory jobHistory);
        void Delete(int id);
        List<JobHistory> GetAll();
        List<JobHistory> GetTop(int howMany);
        List<JobHistory> GetByScheduledJob(int scheduledJobId);
    }
}

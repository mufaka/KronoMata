using KronoMata.Data.Mock;
using KronoMata.Model;

namespace KronoMata.Data.InMemory
{
    public class InMemoryJobHistoryDataStore : InMemoryDataStoreBase, IJobHistoryDataStore
    {
        public InMemoryJobHistoryDataStore(MockDataStoreProvider inMemoryDataStoreProvider, IDataStoreProvider backingDataStoreProvider)
            : base(inMemoryDataStoreProvider, backingDataStoreProvider) 
        {
            ((MockJobHistoryDataStore)inMemoryDataStoreProvider.JobHistoryDataStore)
                .Initialize(backingDataStoreProvider.JobHistoryDataStore.GetAll());
        }

        public JobHistory Create(JobHistory jobHistory)
        {
            var createdJobHistory = BackingDataStoreProvider.JobHistoryDataStore.Create(jobHistory);
            InMemoryDataStoreProvider.JobHistoryDataStore.Create(createdJobHistory);
            return createdJobHistory;
        }

        public void Delete(int id)
        {
            BackingDataStoreProvider.JobHistoryDataStore.Delete(id);
            InMemoryDataStoreProvider.JobHistoryDataStore.Delete(id);
        }

        public List<JobHistory> GetAll()
        {
            return InMemoryDataStoreProvider.JobHistoryDataStore.GetAll();
        }

        public PagedList<JobHistory> GetAllPaged(int pageIndex, int pageSize)
        {
            return InMemoryDataStoreProvider.JobHistoryDataStore.GetAllPaged(pageIndex, pageSize);
        }

        public PagedList<JobHistory> GetFilteredPaged(int pageIndex, int pageSize, int status = -1, int scheduledJobId = -1, int hostId = -1)
        {
            return InMemoryDataStoreProvider.JobHistoryDataStore.GetFilteredPaged(pageIndex, pageSize, status, scheduledJobId, hostId);
        }

        public PagedList<JobHistory> GetByScheduledJob(int scheduledJobId, int pageIndex, int pageSize)
        {
            return InMemoryDataStoreProvider.JobHistoryDataStore.GetByScheduledJob(scheduledJobId, pageIndex, pageSize);
        }

        public PagedList<JobHistory> GetByHost(int hostId, int pageIndex, int pageSize)
        {
            return InMemoryDataStoreProvider.JobHistoryDataStore.GetByHost(hostId, pageIndex, pageSize);
        }

        public List<JobHistory> GetTop(int howMany)
        {
            return InMemoryDataStoreProvider.JobHistoryDataStore.GetTop(howMany);
        }

        public List<JobHistory> GetLastByDate(DateTime startDate)
        {
            return InMemoryDataStoreProvider.JobHistoryDataStore.GetLastByDate(startDate);
        }

        public PagedList<JobHistory> GetLastByDatePaged(DateTime startDate, int pageIndex, int pageSize)
        {
            return InMemoryDataStoreProvider.JobHistoryDataStore.GetLastByDatePaged(startDate, pageIndex, pageSize);
        }
    }
}

using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockScheduledJobDataStore : IScheduledJobDataStore
    {
        private List<ScheduledJob> _scheduledJobs = new List<ScheduledJob>();

        public ScheduledJob Create(ScheduledJob scheduledJob)
        {
            scheduledJob.Id = _scheduledJobs.Count == 0
                ? 1
                : _scheduledJobs[_scheduledJobs.Count - 1].Id + 1;

            _scheduledJobs.Add(scheduledJob);

            return scheduledJob;
        }

        public void Delete(int id)
        {
            _scheduledJobs.RemoveAll(s => s.Id == id);
        }

        public List<ScheduledJob> GetAll()
        {
            return _scheduledJobs;
        }

        public List<ScheduledJob> GetByHost(int hostId)
        {
            return _scheduledJobs.Where(s => s.HostId == hostId).ToList();
        }

        public ScheduledJob GetById(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _scheduledJobs.Where(s => s.Id == id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public List<ScheduledJob> GetByPluginMetaData(int pluginMetaDataId)
        {
            return _scheduledJobs.Where(s => s.PluginMetaDataId == pluginMetaDataId).ToList();
        }

        public void Update(ScheduledJob scheduledJob)
        {
            var existing = _scheduledJobs.Where(s => s.Id == scheduledJob.Id).FirstOrDefault();

            if (existing != null)
            {
                existing = scheduledJob;
            }
        }
    }
}

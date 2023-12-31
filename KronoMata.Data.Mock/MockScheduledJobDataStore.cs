﻿using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockScheduledJobDataStore : BaseMockDataStore, IScheduledJobDataStore
    {
        public MockScheduledJobDataStore(MockDataStoreProvider dataProvider) : base(dataProvider) { }

        private List<ScheduledJob> _scheduledJobs = new();
        public void Initialize(List<ScheduledJob> scheduledJobs) { _scheduledJobs = scheduledJobs; }

        public ScheduledJob Create(ScheduledJob scheduledJob)
        {
            if (scheduledJob.Id <= 0)
            {
                scheduledJob.Id = _scheduledJobs.Count == 0
                    ? 1
                    : _scheduledJobs[^1].Id + 1;
            }

            _scheduledJobs.Add(scheduledJob);

            return scheduledJob;
        }

        public void Delete(int id)
        {
            //_scheduledJobs.RemoveAll(s => s.Id == id);

            var existingIndex = _scheduledJobs.FindIndex(g => g.Id == id);

            if (existingIndex >= 0)
            {
                _scheduledJobs.RemoveAt(existingIndex);
            }

        }

        public List<ScheduledJob> GetAll()
        {
            return _scheduledJobs;
        }

        public List<ScheduledJob> GetByHost(int hostId)
        {
            return _scheduledJobs.Where(s => $",{s.HostIds},".Contains($",{hostId}") || s.HostIds == "-1").ToList();
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
                var existingIndex = _scheduledJobs.FindIndex(g => g.Id == existing.Id);

                if (existingIndex >= 0)
                {
                    _scheduledJobs[existingIndex] = scheduledJob;
                }
            }
        }
    }
}

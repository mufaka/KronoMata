using KronoMata.Model;

namespace KronoMata.Data
{
    /// <summary>
    /// Defines a DataStore for persisting ScheduledJob.
    /// </summary>
    public interface IScheduledJobDataStore
    {
        ScheduledJob Create(ScheduledJob scheduledJob);
        void Update(ScheduledJob scheduledJob);
        void Delete(int id);
        ScheduledJob GetById(int id);
        List<ScheduledJob> GetByHost(int hostId);
        List<ScheduledJob> GetByPluginMetaData(int pluginMetaDataId);
        List<ScheduledJob> GetAll();
    }
}

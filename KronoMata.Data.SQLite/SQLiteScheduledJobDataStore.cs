using KronoMata.Model;

namespace KronoMata.Data.SQLite
{
    public class SQLiteScheduledJobDataStore : SQLiteDataStoreBase, IScheduledJobDataStore
    {
        public ScheduledJob Create(ScheduledJob scheduledJob)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<ScheduledJob> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<ScheduledJob> GetByHost(int hostId)
        {
            throw new NotImplementedException();
        }

        public ScheduledJob GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ScheduledJob> GetByPluginMetaData(int pluginMetaDataId)
        {
            throw new NotImplementedException();
        }

        public void Update(ScheduledJob scheduledJob)
        {
            throw new NotImplementedException();
        }
    }
}

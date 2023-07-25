using KronoMata.Model;

namespace KronoMata.Data
{
    public interface IConfigurationValueDataStore
    {
        ConfigurationValue Create(ConfigurationValue configurationValue);
        void Update(ConfigurationValue configurationValue);
        void Delete(int id);
        List<ConfigurationValue> GetByScheduledJob(int scheduledJobId);
    }
}

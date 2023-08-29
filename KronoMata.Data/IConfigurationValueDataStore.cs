using KronoMata.Model;

namespace KronoMata.Data
{
    /// <summary>
    /// Defines a DataStore for persisting ConfigurationValue.
    /// </summary>
    public interface IConfigurationValueDataStore
    {
        ConfigurationValue Create(ConfigurationValue configurationValue);
        void Update(ConfigurationValue configurationValue);
        void Delete(int id);
        List<ConfigurationValue> GetByScheduledJob(int scheduledJobId);
        List<ConfigurationValue> GetAll();
    }
}

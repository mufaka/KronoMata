using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockConfigurationValueDataStore : IConfigurationValueDataStore
    {
        private readonly List<ConfigurationValue> _configurationValues = new();

        public ConfigurationValue Create(ConfigurationValue configurationValue)
        {
            configurationValue.Id = _configurationValues.Count == 0 
                ? 1 
                : _configurationValues[^1].Id + 1;

            _configurationValues.Add(configurationValue);
            
            return configurationValue;
        }

        public void Delete(int id)
        {
            _configurationValues.RemoveAll(v => v.Id == id);
        }

        public List<ConfigurationValue> GetByScheduledJob(int scheduledJobId)
        {
            return _configurationValues.Where(v => v.ScheduledJobId == scheduledJobId).ToList();
        }

        public void Update(ConfigurationValue configurationValue)
        {
            var existing = _configurationValues.Where(v => v.Id == configurationValue.Id).FirstOrDefault();

            if (existing != null)
            {
                existing = configurationValue;
            }
        }
    }
}

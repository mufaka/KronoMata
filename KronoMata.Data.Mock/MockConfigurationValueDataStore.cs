using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockConfigurationValueDataStore : BaseMockDataStore, IConfigurationValueDataStore
    {
        public MockConfigurationValueDataStore(MockDataStoreProvider dataProvider) : base(dataProvider) { }

        private List<ConfigurationValue> _configurationValues = new();

        public void Initialize(List<ConfigurationValue> configurationValues) { _configurationValues = configurationValues; }

        public ConfigurationValue Create(ConfigurationValue configurationValue)
        {
            if (configurationValue.Id <= 0)
            {
                configurationValue.Id = _configurationValues.Count == 0
                    ? 1
                    : _configurationValues[^1].Id + 1;
            }

            _configurationValues.Add(configurationValue);
            
            return configurationValue;
        }

        public void Delete(int id)
        {
            //_configurationValues.RemoveAll(v => v.Id == id);

            var existingIndex = _configurationValues.FindIndex(g => g.Id == id);

            if (existingIndex >= 0)
            {
                _configurationValues.RemoveAt(existingIndex);
            }
        }

        public List<ConfigurationValue> GetByScheduledJob(int scheduledJobId)
        {
            return _configurationValues.Where(v => v.ScheduledJobId == scheduledJobId).ToList();
        }

        public List<ConfigurationValue> GetAll()
        {
            return _configurationValues;
        }

        public void Update(ConfigurationValue configurationValue)
        {
            var existing = _configurationValues.Where(v => v.Id == configurationValue.Id).FirstOrDefault();

            if (existing != null)
            {
                var existingIndex = _configurationValues.FindIndex(g => g.Id == existing.Id);

                if (existingIndex >= 0)
                {
                    _configurationValues[existingIndex] = configurationValue;
                }
            }
        }
    }
}

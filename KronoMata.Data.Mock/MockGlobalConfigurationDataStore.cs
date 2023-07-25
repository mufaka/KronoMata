using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockGlobalConfigurationDataStore : IGlobalConfigurationDataStore
    {
        private List<GlobalConfiguration> _globalConfigurations = new List<GlobalConfiguration>();

        public GlobalConfiguration Create(GlobalConfiguration globalConfiguration)
        {
            globalConfiguration.Id = _globalConfigurations.Count == 0
                ? 1
                : _globalConfigurations[_globalConfigurations.Count - 1].Id + 1;

            _globalConfigurations.Add(globalConfiguration);

            return globalConfiguration;
        }

        public void Delete(int id)
        {
            _globalConfigurations.RemoveAll(g => g.Id == id);
        }

        public List<GlobalConfiguration> GetAll()
        {
            return _globalConfigurations;
        }

        public List<GlobalConfiguration> GetByCategory(string categoryName)
        {
            return _globalConfigurations.Where(g => g.Category == categoryName).ToList();
        }

        public GlobalConfiguration GetByCategoryAndName(string category, string name)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _globalConfigurations.Where(g => g.Category == category).FirstOrDefault(g => g.Name == name);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void Update(GlobalConfiguration globalConfiguration)
        {
            var existing = _globalConfigurations.Where(g => g.Id == globalConfiguration.Id).FirstOrDefault();

            if (existing != null)
            {
                existing = globalConfiguration;
            }
        }
    }
}

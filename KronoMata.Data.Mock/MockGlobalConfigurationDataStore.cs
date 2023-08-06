using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockGlobalConfigurationDataStore : BaseMockDataStore, IGlobalConfigurationDataStore
    {
        public MockGlobalConfigurationDataStore(MockDataStoreProvider dataProvider) : base(dataProvider) { }

        private readonly List<GlobalConfiguration> _globalConfigurations = new();

        public GlobalConfiguration Create(GlobalConfiguration globalConfiguration)
        {
            globalConfiguration.Id = _globalConfigurations.Count == 0
                ? 1
                : _globalConfigurations[^1].Id + 1;

            _globalConfigurations.Add(globalConfiguration);

            return globalConfiguration;
        }

        public void Delete(int id)
        {
            // this is not working in ASP.NET context for some reason.
            //_globalConfigurations.RemoveAll(g => g.Id == id);

            var existingIndex = _globalConfigurations.FindIndex(g => g.Id == id);

            if (existingIndex >= 0)
            {
                _globalConfigurations.RemoveAt(existingIndex);
            }

        }

        public GlobalConfiguration GetById(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _globalConfigurations.Where(g => g.Id == id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public List<GlobalConfiguration> GetAll()
        {
            return _globalConfigurations.OrderBy(g => g.Id).ToList();
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
                // for some reason this is not working in the ASP.NET context
                // existing = globalConfiguration;

                var existingIndex = _globalConfigurations.FindIndex(g => g.Id == existing.Id);

                if (existingIndex >= 0)
                {
                    _globalConfigurations[existingIndex] = globalConfiguration;
                }
            }
        }
    }
}

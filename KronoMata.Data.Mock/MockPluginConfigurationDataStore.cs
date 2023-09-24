using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockPluginConfigurationDataStore : BaseMockDataStore, IPluginConfigurationDataStore
    {
        public MockPluginConfigurationDataStore(MockDataStoreProvider dataProvider) : base(dataProvider) { }

        private List<PluginConfiguration> _pluginConfigurations = new();

        public void Initialize(List<PluginConfiguration> pluginConfigurations) { _pluginConfigurations = pluginConfigurations; }
        public PluginConfiguration Create(PluginConfiguration pluginConfiguration)
        {
            if (pluginConfiguration.Id <= 0)
            {
                pluginConfiguration.Id = _pluginConfigurations.Count == 0
                    ? 1
                    : _pluginConfigurations[^1].Id + 1;
            }

            _pluginConfigurations.Add(pluginConfiguration);

            return pluginConfiguration;
        }

        public void Delete(int id)
        {
            //_pluginConfigurations.RemoveAll(c => c.Id == id);
            
            var existingIndex = _pluginConfigurations.FindIndex(g => g.Id == id);

            if (existingIndex >= 0)
            {
                _pluginConfigurations.RemoveAt(existingIndex);
            }
        }

        internal void DeleteByPlugin(int pluginMetaDataId)
        {
            _pluginConfigurations.RemoveAll(c => c.PluginMetaDataId == pluginMetaDataId);
        }

        public PluginConfiguration GetById(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _pluginConfigurations.Where(c => c.Id == id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public List<PluginConfiguration> GetByPluginMetaData(int pluginMetaDataId)
        {
            return _pluginConfigurations.Where(c => c.PluginMetaDataId == pluginMetaDataId).ToList();
        }

        public List<PluginConfiguration> GetAll()
        {
            return _pluginConfigurations;
        }

        public void Update(PluginConfiguration pluginConfiguration)
        {
            var existing = _pluginConfigurations.Where(c => c.Id == pluginConfiguration.Id).FirstOrDefault();

            if (existing != null)
            {
                var existingIndex = _pluginConfigurations.FindIndex(g => g.Id == existing.Id);

                if (existingIndex >= 0)
                {
                    _pluginConfigurations[existingIndex] = pluginConfiguration;
                }
            }
        }
    }
}

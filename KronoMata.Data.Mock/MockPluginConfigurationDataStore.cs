using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockPluginConfigurationDataStore : BaseMockDataStore, IPluginConfigurationDataStore
    {
        public MockPluginConfigurationDataStore(MockDataStoreProvider dataProvider) : base(dataProvider) { }

        private readonly List<PluginConfiguration> _pluginConfigurations = new();

        public PluginConfiguration Create(PluginConfiguration pluginConfiguration)
        {
            pluginConfiguration.Id = _pluginConfigurations.Count == 0
                ? 1
                : _pluginConfigurations[^1].Id + 1;

            _pluginConfigurations.Add(pluginConfiguration);

            return pluginConfiguration;
        }

        public void Delete(int id)
        {
            _pluginConfigurations.RemoveAll(c => c.Id == id);
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

        public void Update(PluginConfiguration pluginConfiguration)
        {
            var existing = _pluginConfigurations.Where(c => c.Id == pluginConfiguration.Id).FirstOrDefault();

            if (existing != null)
            {
                existing = pluginConfiguration;
            }
        }
    }
}

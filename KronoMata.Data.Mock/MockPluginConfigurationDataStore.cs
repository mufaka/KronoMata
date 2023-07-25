using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockPluginConfigurationDataStore : IPluginConfigurationDataStore
    {
        public PluginConfiguration Create(PluginConfiguration pluginConfiguration)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public PluginConfiguration GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<PluginConfiguration> GetByPluginMetaData(int pluginMetaDataId)
        {
            throw new NotImplementedException();
        }

        public void Update(PluginConfiguration pluginConfiguration)
        {
            throw new NotImplementedException();
        }
    }
}

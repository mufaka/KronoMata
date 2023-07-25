using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockPluginMetaDataDataStore : IPluginMetaDataDataStore
    {
        public PluginMetaData Create(PluginMetaData pluginMetaData)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public PluginMetaData GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<PluginMetaData> GetByPackageId(int packageId)
        {
            throw new NotImplementedException();
        }

        public void Update(PluginMetaData pluginMetaData)
        {
            throw new NotImplementedException();
        }
    }
}

using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockGlobalConfigurationDataStore : IGlobalConfigurationDataStore
    {
        public GlobalConfiguration Create(GlobalConfiguration globalConfiguration)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<GlobalConfiguration> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<GlobalConfiguration> GetByCategory(string categoryName)
        {
            throw new NotImplementedException();
        }

        public GlobalConfiguration GetByCategoryAndName(string category, string name)
        {
            throw new NotImplementedException();
        }

        public void Update(GlobalConfiguration globalConfiguration)
        {
            throw new NotImplementedException();
        }
    }
}

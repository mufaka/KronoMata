using KronoMata.Model;

namespace KronoMata.Data
{
    public interface IGlobalConfigurationDataStore
    {
        GlobalConfiguration Create(GlobalConfiguration globalConfiguration);
        void Update(GlobalConfiguration globalConfiguration);
        void Delete(int id);
        List<GlobalConfiguration> GetAll();
    }
}

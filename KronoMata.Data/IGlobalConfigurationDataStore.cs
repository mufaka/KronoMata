using KronoMata.Model;

namespace KronoMata.Data
{
    /// <summary>
    /// Defines a DataStore for persisting GlobalConfiguration.
    /// </summary>
    public interface IGlobalConfigurationDataStore
    {
        GlobalConfiguration Create(GlobalConfiguration globalConfiguration);
        void Update(GlobalConfiguration globalConfiguration);
        void Delete(int id);
        List<GlobalConfiguration> GetAll();
        List<GlobalConfiguration> GetByCategory(string categoryName);
        GlobalConfiguration GetByCategoryAndName(string category, string name);
    }
}

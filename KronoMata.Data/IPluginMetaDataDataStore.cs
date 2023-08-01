using KronoMata.Model;

namespace KronoMata.Data
{
    /// <summary>
    /// Defines a DataStore for persisting PluginMetaData.
    /// </summary>
    public interface IPluginMetaDataDataStore
    {
        PluginMetaData Create(PluginMetaData pluginMetaData);
        void Update(PluginMetaData pluginMetaData);
        void Delete(int id);
        PluginMetaData GetById(int id);
        List<PluginMetaData> GetByPackageId(int packageId);

        List<PluginMetaData> GetAll();
    }
}

using KronoMata.Model;

namespace KronoMata.Data
{
    /// <summary>
    /// Defines a DataStore for persisting PluginConfiguration.
    /// </summary>
    public interface IPluginConfigurationDataStore
    {
        PluginConfiguration Create(PluginConfiguration pluginConfiguration);
        void Update(PluginConfiguration pluginConfiguration);
        void Delete(int id);
        PluginConfiguration GetById(int id);
        List<PluginConfiguration> GetByPluginMetaData(int pluginMetaDataId);
    }
}

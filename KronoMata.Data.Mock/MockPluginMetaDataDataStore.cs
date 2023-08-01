﻿using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockPluginMetaDataDataStore : IPluginMetaDataDataStore
    {
        private readonly List<PluginMetaData> _pluginMetaDatas = new();

        public PluginMetaData Create(PluginMetaData pluginMetaData)
        {
            pluginMetaData.Id = _pluginMetaDatas.Count == 0
                ? 1
                : _pluginMetaDatas[^1].Id + 1;

            _pluginMetaDatas.Add(pluginMetaData);

            return pluginMetaData;
        }

        public void Delete(int id)
        {
            _pluginMetaDatas.RemoveAll(p => p.Id == id);
        }

        public PluginMetaData GetById(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _pluginMetaDatas.Where(p => p.Id == id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public List<PluginMetaData> GetByPackageId(int packageId)
        {
            return _pluginMetaDatas.Where(p => p.PackageId == packageId).ToList();
        }

        public void Update(PluginMetaData pluginMetaData)
        {
            var existing = _pluginMetaDatas.Where(p => p.Id == pluginMetaData.Id).FirstOrDefault();

            if (existing != null)
            {
                existing = pluginMetaData;
            }
        }

        public List<PluginMetaData> GetAll()
        {
            return _pluginMetaDatas;
        }
    }
}
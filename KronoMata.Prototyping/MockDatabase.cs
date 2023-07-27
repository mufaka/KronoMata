using KronoMata.Data;
using KronoMata.Data.Mock;
using KronoMata.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronoMata.Prototyping
{
    internal class MockDatabase
    {
        public static readonly MockDatabase Instance = new();
        private IDataStoreProvider _dataProvider;

        private MockDatabase() 
        {
            _dataProvider = new MockDataStoreProvider();
            InitializeData();
        }

        public IDataStoreProvider DataStoreProvider { get { return _dataProvider; } }

        private void InitializeData()
        {
            var now = DateTime.Now;

            Package package = CreatePackage();
            PluginMetaData plugin = CreatePlugin(now, package);
            PluginConfiguration pluginConfiguration1 = CreatePluginConfiguration1(now, plugin);
            PluginConfiguration pluginConfiguration2 = CreatePluginConfiguration2(now, plugin);
            Host host = CreateHost(now);
            ScheduledJob scheduledJob = CreateScheduledJob(now, plugin, host);
            CreateConfigurationValue1(now, pluginConfiguration1, scheduledJob);
            CreateConfigurationValue2(now, pluginConfiguration2, scheduledJob);
        }

        private void CreateConfigurationValue2(DateTime now, PluginConfiguration pluginConfiguration2, ScheduledJob scheduledJob)
        {
            var configurationValue2 = new ConfigurationValue();

            configurationValue2.PluginConfigurationId = pluginConfiguration2.Id;
            configurationValue2.ScheduledJobId = scheduledJob.Id;
            configurationValue2.Value = "Test Detail";
            configurationValue2.InsertDate = now;
            configurationValue2.UpdateDate = now;

            _dataProvider.ConfigurationValueDataStore.Create(configurationValue2);
        }

        private void CreateConfigurationValue1(DateTime now, PluginConfiguration pluginConfiguration1, ScheduledJob scheduledJob)
        {
            var configurationValue1 = new ConfigurationValue();

            configurationValue1.PluginConfigurationId = pluginConfiguration1.Id;
            configurationValue1.ScheduledJobId = scheduledJob.Id;
            configurationValue1.Value = "Test Message";
            configurationValue1.InsertDate = now;
            configurationValue1.UpdateDate = now;

            _dataProvider.ConfigurationValueDataStore.Create(configurationValue1);
        }

        private ScheduledJob CreateScheduledJob(DateTime now, PluginMetaData plugin, Host host)
        {
            var scheduledJob = new ScheduledJob();

            scheduledJob.PluginMetaDataId = plugin.Id;
            scheduledJob.HostId = host.Id;
            scheduledJob.Name = $"{plugin.Name} Testing";
            scheduledJob.Description = "Testing the plugin architecture.";
            scheduledJob.StartTime = now;
            scheduledJob.EndTime = null;
            scheduledJob.Interval = ScheduleInterval.Minute;
            scheduledJob.Step = 2;
            scheduledJob.IsEnabled = true;
            scheduledJob.InsertDate = now;
            scheduledJob.UpdateDate = now;

            _dataProvider.ScheduledJobDataStore.Create(scheduledJob);
            return scheduledJob;
        }

        private PluginConfiguration CreatePluginConfiguration2(DateTime now, PluginMetaData plugin)
        {
            var pluginConfiguration2 = new PluginConfiguration();

            pluginConfiguration2.PluginMetaDataId = plugin.Id;
            pluginConfiguration2.DataType = Public.ConfigurationDataType.String;
            pluginConfiguration2.Name = "EchoDetail";
            pluginConfiguration2.Description = "The detail to echo to the log.";
            pluginConfiguration2.IsRequired = true;
            pluginConfiguration2.InsertDate = now;
            pluginConfiguration2.UpdateDate = now;

            _dataProvider.PluginConfigurationDataStore.Create(pluginConfiguration2);
            return pluginConfiguration2;
        }

        private PluginConfiguration CreatePluginConfiguration1(DateTime now, PluginMetaData plugin)
        {
            var pluginConfiguration1 = new PluginConfiguration();

            pluginConfiguration1.PluginMetaDataId = plugin.Id;
            pluginConfiguration1.DataType = Public.ConfigurationDataType.String;
            pluginConfiguration1.Name = "EchoMessage";
            pluginConfiguration1.Description = "The message to echo to the log.";
            pluginConfiguration1.IsRequired = true;
            pluginConfiguration1.InsertDate = now;
            pluginConfiguration1.UpdateDate = now;

            _dataProvider.PluginConfigurationDataStore.Create(pluginConfiguration1);
            return pluginConfiguration1;
        }

        private Host CreateHost(DateTime now)
        {
            var host = new Host();

            host.MachineName = Environment.MachineName;
            host.IsEnabled = true;
            host.InsertDate = now;
            host.UpdateDate = now;

            _dataProvider.HostDataStore.Create(host);
            return host;
        }

        private PluginMetaData CreatePlugin(DateTime now, Package package)
        {
            var plugin = new PluginMetaData();

            plugin.PackageId = package.Id;
            plugin.AssemblyName = "KronoMata.Samples";
            plugin.ClassName = "KronoMata.Samples.EchoPlugin";
            plugin.Version = "1.0";
            plugin.Name = "Echo Plugin";
            plugin.Description = "A plugin that echos configured text.";
            plugin.InsertDate = now;
            plugin.UpdateDate = now;

            _dataProvider.PluginMetaDataDataStore.Create(plugin);
            return plugin;
        }

        private Package CreatePackage()
        {
            var package = new Package();
            package.FileName = "KronoMata.Samples.zip";

            _dataProvider.PackageDataStore.Create(package);
            return package;
        }
    }
}

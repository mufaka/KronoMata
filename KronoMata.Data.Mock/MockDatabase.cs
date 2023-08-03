using KronoMata.Model;
using System.Net.WebSockets;

namespace KronoMata.Data.Mock
{
    public class MockDatabase
    {
        public static readonly MockDatabase Instance = new();
        private readonly IDataStoreProvider _dataProvider;

        private MockDatabase() 
        {
            _dataProvider = new MockDataStoreProvider();
            InitializeData();
        }

        public IDataStoreProvider DataStoreProvider { get { return _dataProvider; } }

        private void InitializeData()
        {
            var now = DateTime.Now;

            CreateGlobalConfigurations();
            Package package = CreatePackage();
            PluginMetaData plugin = CreatePlugin(now, package);
            PluginConfiguration pluginConfiguration1 = CreatePluginConfiguration1(now, plugin);
            PluginConfiguration pluginConfiguration2 = CreatePluginConfiguration2(now, plugin);
            Host host = CreateHost(now);

            ScheduledJob scheduledJob1 = CreateScheduledJob(now, plugin, host);
            CreateConfigurationValue1(now, pluginConfiguration1, scheduledJob1);
            CreateConfigurationValue2(now, pluginConfiguration2, scheduledJob1);

            ScheduledJob scheduledJob2 = CreateScheduledJob(now, plugin, host);
            CreateConfigurationValue1(now, pluginConfiguration1, scheduledJob2);
            CreateConfigurationValue2(now, pluginConfiguration2, scheduledJob2);

            CreateJobHistories(now, host);
        }

        private void CreateGlobalConfigurations()
        {
            var now = DateTime.Now;

            for (int x = 0; x < 5; x++)
            {
                var config = new GlobalConfiguration()
                {
                    Category = "System",
                    Name = $"Config Name {x + 1}",
                    Value = $"Config Value {x + 1}",
                    IsAccessibleToPlugins = true,
                    IsMasked = false,
                    InsertDate = now,
                    UpdateDate = now
                };

                if (x % 2 == 0)
                {
                    config.IsAccessibleToPlugins = false;
                    config.IsMasked = true;
                }

                _dataProvider.GlobalConfigurationDataStore.Create(config);
            }
        }

        private void CreateJobHistories(DateTime now, Host host)
        {
            var random = new Random();

            foreach (ScheduledJob job in DataStoreProvider.ScheduledJobDataStore.GetAll())
            {
                var startDate = now.AddDays(-7d).Date;
                var runTime = startDate;

                for (int x = 0; x < 7 * 24 ; x++)
                {
                    runTime = runTime.AddMinutes(random.Next(60)).AddSeconds(random.Next(60));

                    var history = new JobHistory();

                    history.HostId = host.Id;
                    history.ScheduledJobId = job.Id;
                    history.RunTime = runTime;
                    history.Status = (ScheduledJobStatus)(random.Next(3));
                    history.Message = $"{history.Status} Message {x + 1}";
                    history.Detail = $"Detail {x + 1}";

                    // step to the next hour
                    runTime = new DateTime(runTime.Year, runTime.Month, runTime.Day, runTime.Hour, 0, 0).AddHours(1);

                    DataStoreProvider.JobHistoryDataStore.Create(history);
                }
            }
        }

        private Package CreatePackage()
        {
            var package = new Package
            {
                FileName = "KronoMata.Samples.zip"
            };

            _dataProvider.PackageDataStore.Create(package);
            return package;
        }
        private PluginMetaData CreatePlugin(DateTime now, Package package)
        {
            var plugin = new PluginMetaData
            {
                PackageId = package.Id,
                AssemblyName = "KronoMata.Samples",
                ClassName = "KronoMata.Samples.EchoPlugin",
                Version = "1.0",
                Name = "Echo Plugin",
                Description = "A plugin that echos configured text.",
                InsertDate = now,
                UpdateDate = now
            };

            _dataProvider.PluginMetaDataDataStore.Create(plugin);
            return plugin;
        }

        private Host CreateHost(DateTime now)
        {
            var host = new Host
            {
                MachineName = Environment.MachineName,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            _dataProvider.HostDataStore.Create(host);
            return host;
        }

        private PluginConfiguration CreatePluginConfiguration1(DateTime now, PluginMetaData plugin)
        {
            var pluginConfiguration1 = new PluginConfiguration
            {
                PluginMetaDataId = plugin.Id,
                DataType = Public.ConfigurationDataType.String,
                Name = "EchoMessage",
                Description = "The message to echo to the log.",
                IsRequired = true,
                InsertDate = now,
                UpdateDate = now
            };

            _dataProvider.PluginConfigurationDataStore.Create(pluginConfiguration1);
            return pluginConfiguration1;
        }

        private PluginConfiguration CreatePluginConfiguration2(DateTime now, PluginMetaData plugin)
        {
            var pluginConfiguration2 = new PluginConfiguration
            {
                PluginMetaDataId = plugin.Id,
                DataType = Public.ConfigurationDataType.String,
                Name = "EchoDetail",
                Description = "The detail to echo to the log.",
                IsRequired = true,
                InsertDate = now,
                UpdateDate = now
            };

            _dataProvider.PluginConfigurationDataStore.Create(pluginConfiguration2);
            return pluginConfiguration2;
        }

        private ScheduledJob CreateScheduledJob(DateTime now, PluginMetaData plugin, Host host)
        {
            var scheduledJob = new ScheduledJob
            {
                PluginMetaDataId = plugin.Id,
                HostId = host.Id,
                Name = $"{plugin.Name} Testing",
                Description = "Testing the plugin architecture.",
                StartTime = now,
                EndTime = null,
                Frequency = ScheduleFrequency.Minute,
                Interval = 2,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            _dataProvider.ScheduledJobDataStore.Create(scheduledJob);
            return scheduledJob;
        }

        private void CreateConfigurationValue1(DateTime now, PluginConfiguration pluginConfiguration1, ScheduledJob scheduledJob)
        {
            var configurationValue1 = new ConfigurationValue
            {
                PluginConfigurationId = pluginConfiguration1.Id,
                ScheduledJobId = scheduledJob.Id,
                Value = $"Test Message {scheduledJob.Id}",
                InsertDate = now,
                UpdateDate = now
            };

            _dataProvider.ConfigurationValueDataStore.Create(configurationValue1);
        }

        private void CreateConfigurationValue2(DateTime now, PluginConfiguration pluginConfiguration2, ScheduledJob scheduledJob)
        {
            var configurationValue2 = new ConfigurationValue
            {
                PluginConfigurationId = pluginConfiguration2.Id,
                ScheduledJobId = scheduledJob.Id,
                Value = $"Test Detail {scheduledJob.Id}",
                InsertDate = now,
                UpdateDate = now
            };

            _dataProvider.ConfigurationValueDataStore.Create(configurationValue2);
        }
    }
}

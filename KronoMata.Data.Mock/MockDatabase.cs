﻿using KronoMata.Model;

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
            Package package = CreatePackage(now);
            PluginMetaData plugin = CreatePlugin(now, package);
            PluginConfiguration pluginConfiguration1 = CreatePluginConfiguration1(now, plugin);
            PluginConfiguration pluginConfiguration2 = CreatePluginConfiguration2(now, plugin);

            CreateOptionalPluginConfigurations(now, plugin);

            Host host1 = CreateHost(now, Environment.MachineName);
            Host host2 = CreateHost(now, "APP_SERVER_02");

            CreateAdditionalHosts(now, 30);

            ScheduledJob scheduledJob1 = CreateScheduledJob(now, plugin, host1, "Echo Test 1");
            CreateConfigurationValue1(now, pluginConfiguration1, scheduledJob1);
            CreateConfigurationValue2(now, pluginConfiguration2, scheduledJob1);

            ScheduledJob scheduledJob2 = CreateScheduledJob(now, plugin, host2, "Echo Test 2");
            CreateConfigurationValue1(now, pluginConfiguration1, scheduledJob2);
            CreateConfigurationValue2(now, pluginConfiguration2, scheduledJob2);

            CreateJobHistories(now, host1, host2);
        }

        private void CreateGlobalConfigurations()
        {
            var now = DateTime.Now;

            for (int x = 0; x < 4; x++)
            {
                var config = new GlobalConfiguration()
                {
                    Category = "System",
                    Name = $"Name {x + 1}",
                    Value = $"Value {x + 1}",
                    IsAccessibleToPlugins = true,
                    IsMasked = false,
                    IsSystemConfiguration = true,
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

            for (int x = 0; x < 10; x++)
            {
                var config = new GlobalConfiguration()
                {
                    Category = "Plugin",
                    Name = $"Name {x + 1}",
                    Value = $"Value {x + 1}",
                    IsAccessibleToPlugins = true,
                    IsMasked = false,
                    InsertDate = now,
                    UpdateDate = now
                };

                _dataProvider.GlobalConfigurationDataStore.Create(config);
            }
        }

        private void CreateJobHistories(DateTime now, Host host1, Host host2)
        {
            var random = new Random();
            int counter = 0;

            var startDate = now.AddMonths(-6);
            var runTime = startDate;

            var days = (DateTime.Now - startDate).Days;

            for (int x = 0; x < days * 24; x++)
            {
                runTime = runTime.AddMinutes(random.Next(30)).AddSeconds(random.Next(30));
                var runTime2 = runTime.AddMinutes(random.Next(30)).AddSeconds(random.Next(30));

                var jobs = DataStoreProvider.ScheduledJobDataStore.GetAll();

                foreach (ScheduledJob job in jobs)
                {
                    counter++;

                    var history = new JobHistory();

                    history.HostId = counter % 2 == 0 ? host2.Id : host1.Id;
                    history.ScheduledJobId = job.Id;
                    history.RunTime = counter % 2 == 0 ? runTime2 : runTime;
                    history.CompletionTime = history.RunTime.AddMilliseconds(random.Next(1, 20000));

                    history.Status = (ScheduledJobStatus)(random.Next(3));
                    history.Message = $"{history.Status} Message {counter}";
                    history.Detail = $"Detail {job.Name} {job.Id} {counter}";

                    DataStoreProvider.JobHistoryDataStore.Create(history);
                }

                // step to the next hour
                runTime = new DateTime(runTime.Year, runTime.Month, runTime.Day, runTime.Hour, 0, 0).AddHours(1);
            }
        }

        private Package CreatePackage(DateTime now)
        {
            var package = new Package
            {
                Name = "KronoMata Sample",
                FileName = "KronoMata.Samples.zip",
                UploadDate = now
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

        private Host CreateHost(DateTime now, string name)
        {
            var host = new Host
            {
                MachineName = name,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now
            };

            _dataProvider.HostDataStore.Create(host);
            return host;
        }

        private void CreateAdditionalHosts(DateTime now, int count)
        {
            for (int x = 1; x <= count; x++)
            {
                bool enabled = x % 12 != 0;
                var host = new Host
                {
                    MachineName = $"SERVER-{x.ToString().PadLeft(2, '0')}",
                    IsEnabled = enabled,
                    InsertDate = now,
                    UpdateDate = now
                };

                _dataProvider.HostDataStore.Create(host);
            }
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

        private void CreateOptionalPluginConfigurations(DateTime now, PluginMetaData plugin)
        {
            // cover all additional data types.
            foreach (Public.ConfigurationDataType dataType in Enum.GetValues(typeof(Public.ConfigurationDataType)))
            {
                var pluginConfiguration = new PluginConfiguration
                {
                    PluginMetaDataId = plugin.Id,
                    DataType = dataType,
                    Name = $"Sample {dataType}",
                    Description = $"A sample {dataType} to use for configuration testing.",
                    IsRequired = true,
                    InsertDate = now,
                    UpdateDate = now
                };

                switch (dataType)
                {
                    case Public.ConfigurationDataType.Select:
                    case Public.ConfigurationDataType.SelectMultiple:
                        pluginConfiguration.SelectValues = "<Choose>,One,Two,Three,Four,Five,Six,Seven,Eight,Nine,Ten";
                        break;
                }

                _dataProvider.PluginConfigurationDataStore.Create(pluginConfiguration);
            }
        }

        private ScheduledJob CreateScheduledJob(DateTime now, PluginMetaData plugin, Host host, string name)
        {
            var scheduledJob = new ScheduledJob
            {
                PluginMetaDataId = plugin.Id,
                HostIds = Convert.ToString(host.Id),
                Name = name,
                Description = "Testing the plugin architecture.",
                StartTime = now,
                EndTime = null,
                Frequency = ScheduleFrequency.Minute,
                Interval = 1,
                IsEnabled = true,
                InsertDate = now,
                UpdateDate = now,
                Days = "5,23,28",
                DaysOfWeek = "Monday,Wednesday,Friday",
                Hours = "2,12,23",
                Minutes = "15,30,45"
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

using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Public;
using KronoMata.Scheduling;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace KronoMata.Prototyping
{
    public class AgentPrototype : IHostedService
    {
        private readonly ILogger<AgentPrototype> _logger;
        private readonly IDataStoreProvider dataStoreProvider;

        private readonly PeriodicTimer _periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        public AgentPrototype(ILogger<AgentPrototype> logger, IDataStoreProvider dsp)
        {
            _logger = logger;
            dataStoreProvider = dsp;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("KronoMata Agent started.");

            await ExecuteAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("KronoMata Agent stopped.");
            _periodicTimer.Dispose();
            return Task.CompletedTask;
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (await _periodicTimer.WaitForNextTickAsync(cancellationToken)
                && !cancellationToken.IsCancellationRequested)
                {
                    CheckForJobs();
                }
            }
            catch (OperationCanceledException) { }
        }

        private void CheckForJobs()
        {
            // is there a host for this system?
            var machineName = Environment.MachineName;
            var host = dataStoreProvider.HostDataStore.GetByMachineName(machineName);
            var recurrence = new RecurrenceShouldRun();

            if (host == null)
            {
                // should self register the host? make configurable.
                Console.WriteLine($"There is no Host defined for {machineName}");
            }
            else
            {
                // are there any scheduled jobs defined for this system?
                var scheduledJobs = dataStoreProvider.ScheduledJobDataStore.GetByHost(host.Id);

                if (scheduledJobs.Count == 0)
                {
                    Console.WriteLine($"There are no scheduled jobs defined for {machineName}");
                }
                else
                {
                    var pluginArchiveRoot = $"PackageRoot{Path.DirectorySeparatorChar}";

                    Parallel.ForEach(scheduledJobs, scheduledJob =>
                    {
                        if (recurrence.ShouldRun(DateTime.Now, scheduledJob))
                        {
                            ExecutePlugin(dataStoreProvider, pluginArchiveRoot, scheduledJob);
                        }
                    });
                }
            }
        }

        private static void ExecutePlugin(IDataStoreProvider dataStoreProvider, string pluginArchiveRoot, ScheduledJob scheduledJob)
        {
            var pluginMetaData = dataStoreProvider.PluginMetaDataDataStore.GetById(scheduledJob.PluginMetaDataId);
            var package = dataStoreProvider.PackageDataStore.GetById(pluginMetaData.PackageId);
            var packageFolder = $"{pluginArchiveRoot}{GetPluginFolderName(pluginMetaData)}";
            var packageArchivePath = $"{pluginArchiveRoot}{package.FileName}";

            // create and extract plugin to package folder
            CreatePackageFolder(packageFolder, packageArchivePath);

            // the work above should result in this folder now being available
            if (!Directory.Exists(packageFolder))
            {
                Console.WriteLine($"Unable to find package folder at {packageFolder}");
            }
            else
            {
                // Map configuration values.
                var systemConfiguration = GetSystemConfiguration(dataStoreProvider);
                var pluginConfiguration = GetScheduledJobConfiguration(dataStoreProvider, scheduledJob, pluginMetaData);

                // Dynamically load plugin. Assembly file must match <AssemblyName>.dll
                var assemblyPath = Path.GetFullPath($"{packageFolder}{Path.DirectorySeparatorChar}{pluginMetaData.AssemblyName}.dll");

                if (!File.Exists(assemblyPath))
                {
                    Console.WriteLine($"Assembly file not found at {assemblyPath}");
                }
                else
                {
                    // McMaster.NETCore.Plugins handles sharing of types nicely.
                    // In .NET Framework you could just do (IPlugin)Assembly.LoadFrom(path).CreateInstance(class name)
                    // but in .NET Core the created instance wasn't assignable to IPlugin
                    var pluginLoader = PluginLoader.CreateFromAssemblyFile(
                        assemblyFile: assemblyPath,
                        sharedTypes: new[] { typeof(IPlugin) },
                        isUnloadable: true);

                    var assembly = pluginLoader.LoadDefaultAssembly();

                    if (assembly != null)
                    {
                        var plugin = assembly.CreateInstance(pluginMetaData.ClassName) as IPlugin;

                        if (plugin != null)
                        {
                            var results = plugin.Execute(systemConfiguration, pluginConfiguration);

                            // can we / should we call Dispose directly? This unloads the loaded assemblies
                            plugin = null;
                            pluginLoader.Dispose();
                            var now = DateTime.Now;

                            foreach (var result in results)
                            {
                                Console.WriteLine($"{now.ToShortDateString()} {now.ToShortTimeString()} IsError: {result.IsError}, Message: {result.Message}, Detail: {result.Detail}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Unable to create an instance of {pluginMetaData.ClassName} from {assemblyPath}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Unable to load assembly from {assemblyPath}");
                    }


                    // TODO: Log results.
                }
            }
        }

        private static void CreatePackageFolder(string packageFolder, string packageArchivePath)
        {
            if (!Directory.Exists(packageFolder))
            {
                if (!File.Exists(packageArchivePath))
                {
                    // TODO: attempt to fetch from future API.
                    Console.WriteLine($"Could not find package path at {packageArchivePath}");
                }
                else
                {
                    // need to extract archive to packageFolder
                    Console.WriteLine($"Found package archive at {packageArchivePath}. Unzipping to {packageFolder}");
                    ZipFile.ExtractToDirectory(packageArchivePath, packageFolder);
                }
            }
        }

        private static Dictionary<string, string> GetScheduledJobConfiguration(IDataStoreProvider dataStoreProvider,
            ScheduledJob scheduledJob, PluginMetaData pluginMetaData)
        {
            var pluginConfiguration = new Dictionary<string, string>();
            var pluginConfigurationList = dataStoreProvider.PluginConfigurationDataStore.GetByPluginMetaData(pluginMetaData.Id);
            var scheduledJobConfigurationList = dataStoreProvider.ConfigurationValueDataStore.GetByScheduledJob(scheduledJob.Id);

            foreach (PluginConfiguration configuration in pluginConfigurationList)
            {
                if (!pluginConfiguration.ContainsKey(configuration.Name))
                {
                    var configurationValue = scheduledJobConfigurationList.Where(c => c.PluginConfigurationId == configuration.Id).FirstOrDefault();

                    if (configurationValue != null)
                    {
                        pluginConfiguration.Add(configuration.Name, configurationValue.Value);
                    }
                }
            }

            return pluginConfiguration;
        }

        private static Dictionary<string, string> GetSystemConfiguration(IDataStoreProvider dataStoreProvider)
        {
            var globalConfigurationList = dataStoreProvider.GlobalConfigurationDataStore.GetAll();
            var systemConfiguration = new Dictionary<string, string>();

            foreach (GlobalConfiguration globalConfiguration in globalConfigurationList)
            {
                if (globalConfiguration.IsAccessibleToPlugins)
                {
                    if (!systemConfiguration.ContainsKey(globalConfiguration.Name))
                    {
                        systemConfiguration.Add(globalConfiguration.Name, globalConfiguration.Value);
                    }
                }
            }

            return systemConfiguration;
        }

        private static string GetPluginFolderName(PluginMetaData metaData)
        {
            // NOTE: Not too worried about Regex performance here as it
            // NOTE: isn't in a hot spot.

            // only allow [0-9a-zA-Z-.] characters in the filename as they
            // are safe on 'all' platforms (Windows, Mac, *nix)

            // replace invalid characters with spaces
            var folderName = Regex.Replace($"{metaData.Name}_{metaData.Version}", @"[^0-9a-zA-Z-.]", " ");

            // replace multiple spaces with a single space
            folderName = Regex.Replace(folderName, "[ ]{2,}", " ");

            // replace single spaces with _
            folderName = Regex.Replace(folderName, "[ ]", "_");

            return folderName;
        }
    }
}

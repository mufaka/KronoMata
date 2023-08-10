using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Public;
using KronoMata.Scheduling;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace KronoMata.Prototyping
{
    public class AgentPrototype : IHostedService
    {
        private readonly ILogger<AgentPrototype> _logger;
        private readonly IDataStoreProvider _dataStoreProvider;
        private readonly IConfiguration _configuration;

        private readonly PeriodicTimer _periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        public AgentPrototype(ILogger<AgentPrototype> logger, IDataStoreProvider dataStoreProvider, IConfiguration configuration)
        {
            _logger = logger;
            _dataStoreProvider = dataStoreProvider;
            _configuration = configuration;
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
            var host = _dataStoreProvider.HostDataStore.GetByMachineName(machineName);
            var recurrence = new RecurrenceShouldRun();

            if (host == null)
            {
                // should self register the host? make configurable.
                _logger.LogWarning($"There is no Host defined for {machineName}");
            }
            else
            {
                // are there any scheduled jobs defined for this system?
                var endpoint = $"Agent/jobs/{machineName}";

                _logger.LogDebug($"Checking for jobs on {RootUrl} at endpoint {endpoint}");
                var scheduledJobs = Get<ScheduledJob>(endpoint);

                if (scheduledJobs.Count == 0)
                {
                    _logger.LogWarning($"There are no scheduled jobs defined for {machineName}");
                }
                else
                {
                    var pluginArchiveRoot = $"PackageRoot{Path.DirectorySeparatorChar}";

                    Parallel.ForEach(scheduledJobs, scheduledJob =>
                    {
                        if (recurrence.ShouldRun(DateTime.Now, scheduledJob))
                        {
                            ExecutePlugin(_dataStoreProvider, pluginArchiveRoot, scheduledJob, host);
                        }
                    });
                }
            }
        }

        private void ExecutePlugin(IDataStoreProvider dataStoreProvider, string pluginArchiveRoot, ScheduledJob scheduledJob,
            KronoMata.Model.Host host)
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
                _logger.LogWarning($"Unable to find package folder at {packageFolder}");
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
                    _logger.LogWarning($"Assembly file not found at {assemblyPath}");
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

                    _logger.LogDebug($"Loading assembly {assemblyPath}");
                    var assembly = pluginLoader.LoadDefaultAssembly();

                    if (assembly != null)
                    {
                        _logger.LogDebug($"Creating an instance of {pluginMetaData.ClassName}");
                        var plugin = assembly.CreateInstance(pluginMetaData.ClassName) as IPlugin;

                        if (plugin != null)
                        {
                            var runTime = DateTime.Now;

                            _logger.LogDebug($"Executing plugin {pluginMetaData.ClassName}");
                            
                            var results = plugin.Execute(systemConfiguration, pluginConfiguration);
                            var completionTime = DateTime.Now;
                            var endpoint = $"Agent/history";

                            foreach (var result in results)
                            {
                                var history = new JobHistory()
                                {
                                    ScheduledJobId = scheduledJob.Id,
                                    HostId = host.Id, // scheduledJob.HostId can be null with run everywhere jobs
                                    Status = result.IsError ? ScheduledJobStatus.Failure : ScheduledJobStatus.Success,
                                    Message = result.Message,
                                    Detail = result.Detail,
                                    RunTime = runTime,
                                    CompletionTime = completionTime
                                };

                                Post<JobHistory>(endpoint, history);
                            }

                            if (plugin is IDisposable)
                            {
                                ((IDisposable)plugin).Dispose();
                            }
                            
                            // should we call Dispose directly? This unloads the loaded assemblies
                            pluginLoader.Dispose();
                            
                            var now = DateTime.Now;

                            foreach (var result in results)
                            {
                                _logger.LogInformation($"{now.ToShortDateString()} {now.ToShortTimeString()} IsError: {result.IsError}, Message: {result.Message}, Detail: {result.Detail}");
                            }
                        }
                        else
                        {
                            _logger.LogError($"Unable to create an instance of {pluginMetaData.ClassName} from {assemblyPath}");
                        }
                    }
                    else
                    {
                        _logger.LogError($"Unable to load assembly from {assemblyPath}");
                    }
                }
            }
        }

        private void CreatePackageFolder(string packageFolder, string packageArchivePath)
        {
            if (!Directory.Exists(packageFolder))
            {
                if (!File.Exists(packageArchivePath))
                {
                    // TODO: attempt to fetch from future API.
                    _logger.LogWarning($"Could not find package path at {packageArchivePath}");
                }
                else
                {
                    // need to extract archive to packageFolder
                    _logger.LogInformation($"Found package archive at {packageArchivePath}. Unzipping to {packageFolder}");
                    ZipFile.ExtractToDirectory(packageArchivePath, packageFolder);
                }
            }
        }

        private Dictionary<string, string> GetScheduledJobConfiguration(IDataStoreProvider dataStoreProvider,
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

        private string RootUrl //= "http://localhost:5002/api/";
        {
            get
            {
                var configurationRoot = _configuration.GetSection("KronoMata");

                if (configurationRoot != null)
                {
                    var apiRoot = configurationRoot.GetSection("APIRoot");

                    if (apiRoot != null)
                    {
                        if (!String.IsNullOrEmpty(apiRoot.Value))
                        {
                            return apiRoot.Value;
                        }
                    }
                }

                throw new ArgumentNullException("APIRoot is not defined in appsettings.json [KronoMata:APIRoot]");
            }
        }

        private string BuildUrl(string endPoint)
        {
            if (RootUrl.EndsWith("/") && !endPoint.StartsWith("/"))
            {
                return RootUrl + endPoint;
            }
            else if (!RootUrl.EndsWith("/") && endPoint.StartsWith("/"))
            {
                return RootUrl + endPoint;
            }
            else if (RootUrl.EndsWith("/") && endPoint.StartsWith("/"))
            {
                return RootUrl + endPoint.Substring(1);
            }
            else
            {
                return RootUrl + "/" + endPoint;
            }
        }

        public List<T> Get<T>(string endPoint)
        {
            using (var client = new HttpClient())
            {
                // why is there no Get? have to use Async?
                using (var response = client.GetAsync(BuildUrl(endPoint)))
                {
                    // synchronous hackery ... :(
                    response.Wait();
                    var content = response.Result.Content.ReadAsStringAsync();
                    content.Wait();

#pragma warning disable CS8603 // Possible null reference return.
                    return JsonConvert.DeserializeObject<List<T>>(content.Result);
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
        }

        public T Post<T>(string endPoint, T t)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(t), Encoding.UTF8, "application/json");
                var response = client.PostAsync(BuildUrl(endPoint), content);
                response.Wait();
                var responseContent = response.Result.Content.ReadAsStringAsync();
                responseContent.Wait();

#pragma warning disable CS8603 // Possible null reference return.
                return JsonConvert.DeserializeObject<T>(responseContent.Result);
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

    }
}

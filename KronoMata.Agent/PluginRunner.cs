using KronoMata.Model;
using KronoMata.Public;
using KronoMata.Scheduling;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace KronoMata.Agent
{
    internal class PluginRunner : IHostedService
    {
        private readonly ILogger<PluginRunner> _logger;
        private readonly IConfiguration _configuration;
        private readonly IShouldRun _shouldRun;
        private readonly PeriodicTimer _periodicTimer = new(TimeSpan.FromMinutes(1));

        public PluginRunner(ILogger<PluginRunner> logger, IConfiguration configuration, IShouldRun shouldRun)
        {
            _logger = logger;
            _configuration = configuration;
            _shouldRun = shouldRun;
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
                    _logger.LogDebug("Agent checking for jobs.");

                    try
                    {
                        CheckForJobs();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error checking for jobs. {ex.Message}", ex.Message);
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected errror executing agent. {ex.Message", ex.Message);
            }
        }

        private void CheckForJobs()
        {
            var apiClient = new ApiClient(_configuration);
            var machineName = Environment.MachineName;

            _logger.LogDebug("Checking API for scheduled jobs");
            var scheduledJobs = apiClient.GetScheduledJobs(machineName);

            if (scheduledJobs.Count == 0)
            {
                _logger.LogWarning("There are no scheduled jobs defined for {machineName}", machineName);
            }
            else
            {

                var packageRoot = _configuration["KronoMata:PackageRoot"];

                if (String.IsNullOrEmpty(packageRoot))
                {
                    throw new ArgumentNullException("PackageRoot is not defined in appsettings.json [KronoMata:PackageRoot]");
                }

                var pluginArchiveRoot = $"PackageRoot{Path.DirectorySeparatorChar}";
                var host = apiClient.GetHost(machineName);

                _logger.LogDebug("{scheduledJobs.Count} jobs are defined for this Host.", scheduledJobs.Count);

                if (host != null)
                {
                    Parallel.ForEach(scheduledJobs, scheduledJob =>
                    {
                        if (_shouldRun.ShouldRun(DateTime.Now, scheduledJob))
                        {
                            var runTime = DateTime.Now;
                            var results = new List<PluginResult>();
                            try
                            {
                                results.AddRange(ExecutePlugin(apiClient, pluginArchiveRoot, scheduledJob));
                            } 
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Unexpected error executing plugin. {ex.Message}", ex.Message);

                                results.Add(new PluginResult()
                                {
                                    IsError = true,
                                    Message = ex.Message,
                                    Detail = ex.ToString()
                                });
                            }

                            var completionTime = DateTime.Now;
                            SavePluginResults(apiClient, runTime, completionTime, host, scheduledJob, results);
                        }
                        else
                        {
                            _logger.LogDebug("Scheduled Job {scheduledJob.Name} shouldn't be run at this time.", scheduledJob.Name);
                        }
                    });
                }
                else
                {
                    _logger.LogWarning("Unable to get Host for {machineName}. No jobs will be run.", machineName);
                }
            }
        }

        private void SavePluginResults(ApiClient apiClient, DateTime runTime, DateTime completionTime, 
            Model.Host host, ScheduledJob scheduledJob, List<PluginResult> results)
        {
            foreach (var result in results)
            {
                try
                {
                    var history = new JobHistory()
                    {
                        ScheduledJobId = scheduledJob.Id,
                        HostId = host.Id,
                        Status = result.IsError ? ScheduledJobStatus.Failure : ScheduledJobStatus.Success,
                        Message = result.Message,
                        Detail = result.Detail,
                        RunTime = runTime,
                        CompletionTime = completionTime
                    };

                    apiClient.SaveJobHistory(history);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Unable to save job history. {ex.Message}", ex.Message);
                }
            }
        }

        private void SaveSkippedJobHistory(ApiClient apiClient, DateTime runTime, DateTime completionTime, 
            int hostId, int scheduledJobId, string message, string detail)
        {
            try
            {
                var history = new JobHistory()
                {
                    ScheduledJobId = scheduledJobId,
                    HostId = hostId,
                    Status = ScheduledJobStatus.Skipped,
                    Message = message,
                    Detail = detail,
                    RunTime = runTime,
                    CompletionTime = completionTime
                };

                apiClient.SaveJobHistory(history);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unable to save job history. {ex.Message}", ex.Message);
            }
        }

        private List<PluginResult> ExecutePlugin(ApiClient apiClient, string pluginArchiveRoot, ScheduledJob scheduledJob)
        {
            var pluginResults = new List<PluginResult>();

            var pluginMetaData = apiClient.GetPluginMetaData(scheduledJob.PluginMetaDataId);
            if (pluginMetaData == null)
            {
                _logger.LogCritical("Unable to get PluginMetaData with id {scheduledJob.PluginMetaDataID} from API.", scheduledJob.PluginMetaDataId);
                throw new ApplicationException("Plugin not found.");
            }

            var package = apiClient.GetPackage(pluginMetaData.PackageId);
            if (package == null)
            {
                _logger.LogCritical("Unable to get Package with id {pluginMetaData.PackageId} from API.", pluginMetaData.PackageId);
                throw new ApplicationException("Package not found.");
            }

            var packageFolder = $"{pluginArchiveRoot}{GetPluginFolderName(pluginMetaData)}";
            var packageArchivePath = $"{pluginArchiveRoot}{package.FileName}";

            // create and extract plugin to package folder
            CreatePackageFolder(packageFolder, packageArchivePath);

            // the work above should result in this folder now being available
            if (!Directory.Exists(packageFolder))
            {
                _logger.LogCritical("Unable to find package folder at {packageFolder}", packageFolder);
                throw new ApplicationException("Unable to find package folder.");
            }
            else
            {
                // Map configuration values.
                var systemConfiguration = GetSystemConfiguration(apiClient);
                var pluginConfiguration = GetScheduledJobConfiguration(apiClient, scheduledJob, pluginMetaData);

                // Dynamically load plugin. Assembly file must match <AssemblyName>.dll
                var assemblyPath = Path.GetFullPath($"{packageFolder}{Path.DirectorySeparatorChar}{pluginMetaData.AssemblyName}.dll");

                if (!File.Exists(assemblyPath))
                {
                    _logger.LogCritical("Assembly file not found at {assemblyPath}", assemblyPath);
                    throw new ApplicationException("Assembly file not found.");
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

                    _logger.LogDebug("Loading assembly {assemblyPath}", assemblyPath);
                    var assembly = pluginLoader.LoadDefaultAssembly();

                    if (assembly != null)
                    {
                        _logger.LogDebug("Creating an instance of {pluginMetaData.ClassName}", pluginMetaData.ClassName);

                        if (assembly.CreateInstance(pluginMetaData.ClassName) is IPlugin plugin)
                        {
                            var runTime = DateTime.Now;

                            _logger.LogDebug("Executing plugin {pluginMetaData.ClassName}", pluginMetaData.ClassName);

                            var results = plugin.Execute(systemConfiguration, pluginConfiguration);
                            pluginResults.AddRange(results);

                            if (plugin is IDisposable disposable)
                            {
                                disposable.Dispose();
                            }

                            // should we call Dispose directly? This unloads the loaded assemblies
                            pluginLoader.Dispose();
                        }
                        else
                        {
                            _logger.LogError("Unable to create an instance of {pluginMetaData.ClassName} from {assemblyPath}", pluginMetaData.ClassName, assemblyPath);
                            pluginResults.Add(new PluginResult()
                            {
                                IsError = true,
                                Message = "Unable to create an instance of Plugin",
                                Detail = $"Unable to create an instance of {pluginMetaData.ClassName} from {assemblyPath}"
                            });
                        }
                    }
                    else
                    {
                        _logger.LogError("Unable to load assembly from {assemblyPath}", assemblyPath);
                        pluginResults.Add(new PluginResult()
                        {
                            IsError = true,
                            Message = "Unable to load plugin assembly.",
                            Detail = $"Unable to load assembly from {assemblyPath}"
                        });
                    }
                }
            }

            return pluginResults;
        }

        private static Dictionary<string, string> GetSystemConfiguration(ApiClient apiClient)
        {
            var globalConfigurationList = apiClient.GetGlobalConfigurations();
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

        private static Dictionary<string, string> GetScheduledJobConfiguration(ApiClient apiClient,
            ScheduledJob scheduledJob, PluginMetaData pluginMetaData)
        {
            var pluginConfiguration = new Dictionary<string, string>();
            var pluginConfigurationList = apiClient.GetPluginConfigurations(pluginMetaData.Id);
            var scheduledJobConfigurationList = apiClient.GetConfigurationValues(scheduledJob.Id);

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

        private void CreatePackageFolder(string packageFolder, string packageArchivePath)
        {
            if (!Directory.Exists(packageFolder))
            {
                if (!File.Exists(packageArchivePath))
                {
                    // TODO: attempt to fetch zip file from API.
                    _logger.LogWarning("Could not find package path at {packageArchivePath}", packageArchivePath);
                }
                else
                {
                    // need to extract archive to packageFolder
                    _logger.LogInformation("Found package archive at {packageArchivePath}. Unzipping to {packageFolder}", packageArchivePath, packageFolder);
                    ZipFile.ExtractToDirectory(packageArchivePath, packageFolder);
                }
            }
        }
    }
}

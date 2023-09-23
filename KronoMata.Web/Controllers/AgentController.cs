using KronoMata.Data;
using KronoMata.Model;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly ILogger<AgentController> _logger;
#pragma warning disable IDE0052 // Remove unread private members
        private readonly IConfiguration _configuration;
#pragma warning restore IDE0052 // Remove unread private members
        private IDataStoreProvider DataStoreProvider { get; set; }
        private readonly IDataProtector _dataProtector;

        public AgentController(ILogger<AgentController> logger, IDataStoreProvider dataStoreProvider,
            IConfiguration configuration, IDataProtectionProvider dataProtectionProvider)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            _configuration = configuration;
            _dataProtector = dataProtectionProvider.CreateProtector("KronoMata.Web.v1");
        }

        [HttpGet("jobs/{name}")]
        public IEnumerable<ScheduledJob> GetByHostName(string name)
        {
            var jobs = new List<ScheduledJob>();
            var now = DateTime.Now;

            _logger.LogDebug("API getting host by name {name}", name);

            try
            {
                var host = DataStoreProvider.HostDataStore.GetByMachineName(name);

                if (host != null)
                {
                    if (host.IsEnabled)
                    {
                        try
                        {
                            // update the Host's UpdateDate so that we can track last accessed
                            // time without having to use another datetime column.
                            host.UpdateDate = now;
                            DataStoreProvider.HostDataStore.Update(host);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning("Unable to update the last accessed time for Host {host.MachineName}. {ex.Message}", host.MachineName, ex.Message);
                        }

                        jobs = DataStoreProvider.ScheduledJobDataStore.GetByHost(host.Id).Where(h => h.IsEnabled).ToList();
                    }
                } 
                else
                {
                    _logger.LogDebug("API creating host by name {name}", name);

                    var newHost = new Model.Host()
                    {
                        MachineName = name,
                        IsEnabled = false,
                        InsertDate = now,
                        UpdateDate = now
                    };

                    DataStoreProvider.HostDataStore.Create(newHost);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting jobs by host name {name}", name);
            }

            return jobs;
        }

        [HttpPost("history")]
        public JobHistory Post([FromBody] JobHistory history)
        {
            try
            {
                _logger.LogDebug("API saving Job History");
                DataStoreProvider.JobHistoryDataStore.Create(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating JobHistory");
            }

            return history;
        }

        [HttpGet("host/{name}")]
        public List<Model.Host> GetHost(string name)
        {
            var list = new List<Model.Host>();

            try
            {
                _logger.LogDebug("API getting host by name {name}", name);
                var host = DataStoreProvider.HostDataStore.GetByMachineName(name);

                if (host != null)
                {
                    list.Add(host);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting host by name {name}", name);
            }

            return list;
        }

        [HttpGet("plugin/{id}")]
        public List<PluginMetaData> GetPluginMetaData(int id)
        {
            var list = new List<PluginMetaData>();

            try
            {
                _logger.LogDebug("API getting plugin with id {id}", id);
                var plugin = DataStoreProvider.PluginMetaDataDataStore.GetById(id);

                if (plugin != null)
                {
                    list.Add(plugin);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting plugin by id {id}", id);
            }

            return list;
        }

        [HttpGet("package/{id}")]
        public List<Package> GetPackage(int id)
        {
            var list = new List<Package>();

            try
            {
                _logger.LogDebug("API getting package with id {id}", id);
                var package = DataStoreProvider.PackageDataStore.GetById(id);

                if (package != null)
                {
                    list.Add(package);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting package by id {id}", id);
            }

            return list;
        }

        [HttpGet("package/")]
        public List<Package> GetAllPackages()
        {
            var list = new List<Package>();

            try
            {
                _logger.LogDebug("API getting all packages.");
                list = DataStoreProvider.PackageDataStore.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all packages.");
            }

            return list;
        }


        [HttpGet("globalconfig")]
        public List<GlobalConfiguration> GetGlobalConfigurations()
        {
            var list = new List<GlobalConfiguration>();

            try
            {
                _logger.LogDebug("API getting global configuration");
                list = DataStoreProvider.GlobalConfigurationDataStore.GetAll().Where(c => c.IsAccessibleToPlugins).ToList();

                foreach (GlobalConfiguration globalConfiguration in list)
                {
                    if (globalConfiguration.IsMasked)
                    {
                        try
                        {
                            globalConfiguration.Value = _dataProtector.Unprotect(globalConfiguration.Value);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning("Unable to decrypt Global Configuration Value for Category {globalConfiguration.Category}, Name {globalConfiguration.Name}. {ex.Message}"
                                , globalConfiguration.Category, globalConfiguration.Name, ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting global configuration");
            }

            return list;
        }

        [HttpGet("pluginconfig/{pluginMetaDataId}")]
        public List<PluginConfiguration> GetPluginConfigurations(int pluginMetaDataId)
        {
            var list = new List<PluginConfiguration>();

            try
            {
                _logger.LogDebug("API getting plugin configuration for plugin id {pluginMetaDataId}", pluginMetaDataId);
                list = DataStoreProvider.PluginConfigurationDataStore.GetByPluginMetaData(pluginMetaDataId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting plugin configuration");
            }

            return list;
        }

        [HttpGet("configvalue/{scheduledJobId}")]
        public List<ConfigurationValue> GetConfigurationValues(int scheduledJobId)
        {
            var list = new List<ConfigurationValue>();
            var pluginConfigurationList = DataStoreProvider.PluginConfigurationDataStore.GetAll();

            try
            {
                _logger.LogDebug("API getting configuration values for scheduled job id {scheduledJobId}", scheduledJobId);
                list = DataStoreProvider.ConfigurationValueDataStore.GetByScheduledJob(scheduledJobId);

                foreach (ConfigurationValue configurationValue in list)
                {
                    var pluginConfiguration = pluginConfigurationList.Where(p => p.Id == configurationValue.PluginConfigurationId).FirstOrDefault();

                    if (pluginConfiguration != null && pluginConfiguration.DataType == Public.ConfigurationDataType.Password)
                    {
                        try
                        {
                            configurationValue.Value = _dataProtector.Unprotect(configurationValue.Value);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning("Unable to decrypt Configuration Value for Name {pluginConfiguration.Name}. {ex.Message}"
                                , pluginConfiguration.Name, ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting configuration values for scheduled job id {scheduledJobId}.", scheduledJobId);
            }

            return list;
        }

        [HttpGet("package/file/{packageId}")]
        public ActionResult GetPackageFile(int packageId)
        {
            try
            {
                var package = DataStoreProvider.PackageDataStore.GetById(packageId);
                var packageRoot = _configuration["KronoMata:PackageRoot"];

                if (String.IsNullOrEmpty(packageRoot))
                {
                    throw new ArgumentNullException("PackageRoot is not defined in appsettings.json [KronoMata:PackageRoot]");
                }

                if (!packageRoot.EndsWith(Path.DirectorySeparatorChar.ToString())) packageRoot += Path.DirectorySeparatorChar;
                var packageArchivePath = $"{packageRoot}{package.FileName}";

                if (!System.IO.File.Exists(packageArchivePath))
                {
                    throw new ApplicationException($"Package file is missing from {packageArchivePath}");
                }

                var fileStream = System.IO.File.OpenRead(packageArchivePath);

                return File(fileStream, "application/zip", package.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error serving package file for package id {pacakgeId}.", packageId);
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }
    }
}

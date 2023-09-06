using FluentValidation;
using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    [Authorize]
    public class ScheduledJobController : BaseController
    {
        private readonly ILogger<ScheduledJobController> _logger;
        private readonly IValidator _scheduledJobValidator;
        private readonly IDataProtector _dataProtector;

        public ScheduledJobController(ILogger<ScheduledJobController> logger, IDataStoreProvider dataStoreProvider,
            IConfiguration configuration, IValidator<ScheduledJob> scheduledJobValidator, IDataProtectionProvider dataProtectionProvider)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
            _scheduledJobValidator = scheduledJobValidator;
            _dataProtector = dataProtectionProvider.CreateProtector("KronoMata.Web.v1");
        }

        public IActionResult Index()
        {
            var model = new ScheduledJobViewModel()
            {
                ViewName = "Scheduled Jobs"
            };

            try
            {
                // TODO: Should we page this as well?
                model.ScheduledJobs = DataStoreProvider.ScheduledJobDataStore.GetAll()
                    .OrderByDescending(s => s.IsEnabled)
                    .ThenBy(s => s.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
            }
            return View(model);
        }

        public ActionResult GetScheduledJobData()
        {
            try
            {
                var allHosts = DataStoreProvider.HostDataStore.GetAll();

                var jobs = DataStoreProvider.ScheduledJobDataStore.GetAll()
                    .OrderByDescending(s => s.IsEnabled)
                    .ThenBy(s => s.Name)
                    .ToList();

                List<MultipleHost> hosts = GetUniqueHostNames(allHosts, jobs);

                var db = Json(new
                {
                    hosts,
                    jobs
                });

                return Json(new
                {
                    data = db
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Scheduled Job data.");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }

        // mock what jsGrid is expecting for Host
        private class MultipleHost
        {
            public string Id { get; set; } = String.Empty;
            public string MachineName { get; set; } = String.Empty;
        }

        private static List<MultipleHost> GetUniqueHostNames(List<Model.Host> allHosts, List<ScheduledJob> jobs)
        {
            var uniqueMultiHosts = new Dictionary<string, MultipleHost>();

            foreach (ScheduledJob scheduledJob in jobs)
            {
                if (!uniqueMultiHosts.ContainsKey(scheduledJob.HostIds))
                {
                    if (scheduledJob.HostIds == "-1")
                    {
                        uniqueMultiHosts.Add("-1", new MultipleHost() { Id = "-1", MachineName = "All" });
                    }
                    else
                    {
                        // get integer host id's from csv
                        List<int> hostIds = new List<int>(Array.ConvertAll(scheduledJob.HostIds.Split(','), int.Parse));

                        // get hosts that match those ids
                        var matchingHosts = allHosts.Where(h => hostIds.Contains(h.Id)).ToList();

                        // get list of names
                        var names = String.Join(" ", matchingHosts.Select(h => h.MachineName));

                        uniqueMultiHosts.Add(scheduledJob.HostIds, new MultipleHost() { Id = scheduledJob.HostIds, MachineName = names });
                    }
                }
            }

            return uniqueMultiHosts.Values.ToList();
        }

        private void PopulateCommonProperties(ScheduledJobSaveViewModel model, bool updating)
        {
            var hosts = DataStoreProvider.HostDataStore.GetAll()
                .Where(h => h.IsEnabled || updating)
                .OrderBy(h => h.MachineName)
                .ToList();

            var allHost = new Model.Host()
            {
                Id = -1,
                MachineName = "<All>"
            };

            hosts.Insert(0, allHost);
            model.Hosts = hosts;

            var plugins = DataStoreProvider.PluginMetaDataDataStore.GetAll().OrderBy(p => p.Name).ToList();

            if (!updating)
            {
                var choosePlugin = new PluginMetaData()
                {
                    Id = 0,
                    Name = "<Choose>"
                };

                plugins.Insert(0, choosePlugin);
            }


            model.Plugins = plugins;
        }

        public ActionResult Create()
        {
            var model = new ScheduledJobSaveViewModel()
            {
                ViewName = "Scheduled Job Create",
#pragma warning disable CS8601 // Possible null reference assignment.
                ActionUrl = Url.Action("CreateJob", "ScheduledJob")
#pragma warning restore CS8601 // Possible null reference assignment.
            };

            try
            {
                PopulateCommonProperties(model, false);
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
            }

            return View("Save", model);
        }

        [HttpPost]
        public ActionResult CreateJob(ScheduledJob scheduledJob)
        {
            var model = new ScheduledJobSaveViewModel()
            {
                ViewName = "Scheduled Job Create",
#pragma warning disable CS8601 // Possible null reference assignment.
                ActionUrl = Url.Action("Create", "ScheduledJob")
#pragma warning restore CS8601 // Possible null reference assignment.
            };

            try
            {
                scheduledJob.InsertDate = DateTime.Now;
                scheduledJob.UpdateDate = scheduledJob.InsertDate;

                var validationResult = _scheduledJobValidator.Validate(new ValidationContext<ScheduledJob>(scheduledJob));

                if (!validationResult.IsValid)
                {
                    return GetValidationErrorResponse(validationResult);
                }
                else
                {
                    // if the All hosts is one of the selections, default to just All
                    if (String.IsNullOrWhiteSpace(scheduledJob.HostIds) || scheduledJob.HostIds.Split(",".ToCharArray()).Contains("-1"))
                    {
                        scheduledJob.HostIds = "-1";
                    }

                    DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob);

                    // redirect to configure if the plugin has configuration defined.
                    var pluginConfigurations = DataStoreProvider.PluginConfigurationDataStore.GetByPluginMetaData(scheduledJob.PluginMetaDataId);

                    var redirect = pluginConfigurations.Count > 0 ? "configure" : "index";
                    var response = new
                    {
                        redirect,
                        id = scheduledJob.Id
                    };
                    return new ObjectResult(response) { StatusCode = 200 };
                }
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
                PopulateCommonProperties(model, false);
                model.ScheduledJob = scheduledJob;
                return View("Save", model);
            }
        }

        public ActionResult Update(int id)
        {
            var model = new ScheduledJobSaveViewModel() {
                ViewName = "Scheduled Job Update",
#pragma warning disable CS8601 // Possible null reference assignment.
                ActionUrl = Url.Action("Update", "ScheduledJob")
#pragma warning restore CS8601 // Possible null reference assignment.
            };

            try
            {
                PopulateCommonProperties(model, true);
                model.ScheduledJob = DataStoreProvider.ScheduledJobDataStore.GetById(id);
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
            }

            return View("Save", model);
        }

        [HttpPost]
        public ActionResult Update(ScheduledJob scheduledJob)
        {

            var model = new ScheduledJobSaveViewModel()
            {

                ViewName = "Scheduled Job Update",
#pragma warning disable CS8601 // Possible null reference assignment.
                ActionUrl = Url.Action("Update", "ScheduledJob")
#pragma warning restore CS8601 // Possible null reference assignment.
            };

            try
            {
                var existing = DataStoreProvider.ScheduledJobDataStore.GetById(scheduledJob.Id);

                if (existing != null)
                {
                    scheduledJob.InsertDate = existing.InsertDate;
                    scheduledJob.UpdateDate = DateTime.Now;

                    var validationResult = _scheduledJobValidator.Validate(new ValidationContext<ScheduledJob>(scheduledJob));

                    if (!validationResult.IsValid)
                    {
                        return GetValidationErrorResponse(validationResult);
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(scheduledJob.HostIds) || scheduledJob.HostIds.Split(",".ToCharArray()).Contains("-1"))
                        {
                            scheduledJob.HostIds = "-1";
                        }

                        DataStoreProvider.ScheduledJobDataStore.Update(scheduledJob);

                        // redirect to configure if there is no ConfigurationValues defined or the Plugin has changed
                        var redirect = "index";
                        var pluginConfigurations = DataStoreProvider.PluginConfigurationDataStore.GetByPluginMetaData(scheduledJob.PluginMetaDataId);

                        if (pluginConfigurations.Count > 0)
                        {
                            var configurationValues = DataStoreProvider.ConfigurationValueDataStore.GetByScheduledJob(scheduledJob.Id);

                            if (configurationValues.Count == 0 || existing.PluginMetaDataId != scheduledJob.PluginMetaDataId)
                            {
                                redirect = "configure";
                            }
                        }

                        var response = new
                        {
                            redirect,
                            id = scheduledJob.Id
                        };

                        return new ObjectResult(response) { StatusCode = 200 };
                    }
                }
                else
                {
                    _logger.LogError("Unable to get ScheduledJob with Id {id}", scheduledJob.Id);
                    return new ObjectResult($"Unable to get ScheduledJob with Id {scheduledJob.Id}") { StatusCode = 500 };
                }
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);

                PopulateCommonProperties(model, true);
                model.ScheduledJob = scheduledJob;

                return View("Save", model);
            }
        }

        public ActionResult Configure(int id)
        {
            var model = new ConfigureScheduledJobViewModel()
            {
                ViewName = "Scheduled Job Configuration"
            };

            try
            {
                model.ScheduledJob = DataStoreProvider.ScheduledJobDataStore.GetById(id);

                if (model.ScheduledJob == null)
                {
                    model.ScheduledJob = new ScheduledJob() { Name = "Invalid Scheduled Job" };
                    throw new ArgumentException($"Invalid Scheduled Job ID [{id}] provided.");
                }
                else
                {
                    model.PluginConfigurations = DataStoreProvider.PluginConfigurationDataStore.GetByPluginMetaData(model.ScheduledJob.PluginMetaDataId);

                    var existingConfigurationValues = DataStoreProvider.ConfigurationValueDataStore.GetByScheduledJob(id);
                    var configurationValues = new List<ConfigurationValue>();

                    if (model.PluginConfigurations.Count > 0)
                    {
                        foreach (PluginConfiguration pluginConfiguration in model.PluginConfigurations)
                        {
                            var existingConfigurationValue = existingConfigurationValues.Where(c => c.PluginConfigurationId == pluginConfiguration.Id).FirstOrDefault();

                            if (existingConfigurationValue == null)
                            {
                                existingConfigurationValue = new ConfigurationValue
                                {
                                    ScheduledJobId = id,
                                    PluginConfigurationId = pluginConfiguration.Id,
                                    InsertDate = DateTime.Now
                                };
                                existingConfigurationValue.UpdateDate = existingConfigurationValue.InsertDate;
                            }
                            else
                            {
                                try
                                {
                                    if (pluginConfiguration.DataType == Public.ConfigurationDataType.Password)
                                    {
                                        existingConfigurationValue.Value = _dataProtector.Unprotect(existingConfigurationValue.Value);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning("Unable to decrypt Configuration Value for Name {pluginConfiguration.Name}. {ex.Message}"
                                        , pluginConfiguration.Name, ex.Message);
                                }
                            }

                            configurationValues.Add(existingConfigurationValue);
                        }
                    }

                    model.ConfigurationValues = configurationValues;
                }
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveConfiguration(JobConfigSaveModel saveModel)
        {
            var model = new ConfigureScheduledJobViewModel
            {
                ViewName = "Scheduled Job Configuration"
            };

            try
            {
                var now = DateTime.Now;
                var validConfigurationValues = new List<ConfigurationValue>();
                var scheduledJob = DataStoreProvider.ScheduledJobDataStore.GetById(saveModel.ScheduledJobId);

                foreach (var pluginConfigValue in saveModel.PluginConfigValues)
                {
                    var pluginConfiguration = DataStoreProvider.PluginConfigurationDataStore
                        .GetById(pluginConfigValue.PluginConfigurationId);

                    var existingConfigurationValue = validConfigurationValues
                        .Where(c => c.PluginConfigurationId == pluginConfigValue.PluginConfigurationId)
                        .FirstOrDefault();

                    // ignore "_empty_" values for Multi-Select
                    if (pluginConfiguration.DataType == Public.ConfigurationDataType.SelectMultiple
                        && pluginConfigValue.Value == "_empty_")
                    {
                        pluginConfigValue.Value = String.Empty;
                    }

                    if (existingConfigurationValue == null)
                    {
                        existingConfigurationValue = new ConfigurationValue()
                        {
                            Id = pluginConfigValue.ConfigurationValueId,
                            PluginConfigurationId = pluginConfigValue.PluginConfigurationId,
                            ScheduledJobId = saveModel.ScheduledJobId,
                            InsertDate = now, // TODO: this should maintain original Insert Date
                            UpdateDate = now,
                            Value = pluginConfigValue.Value
                        };
                    }
                    else
                    {
                        // only update if new value is not empty
                        if (!String.IsNullOrWhiteSpace(pluginConfigValue.Value))
                        {
                            // if the existing value is not empty, append to it
                            existingConfigurationValue.Value = String.IsNullOrWhiteSpace(existingConfigurationValue.Value)
                                ? pluginConfigValue.Value
                                : $"{existingConfigurationValue.Value},{pluginConfigValue.Value}";
                        }
                    }

                    var validationMessage = ValidateConfigurationValue(pluginConfiguration, existingConfigurationValue);

                    if (pluginConfiguration.DataType == Public.ConfigurationDataType.Password)
                    {
                        try
                        {
                            if (pluginConfiguration.DataType == Public.ConfigurationDataType.Password)
                            {
                                existingConfigurationValue.Value = _dataProtector.Protect(existingConfigurationValue.Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning("Unable to encrypt Configuration Value for Name {pluginConfiguration.Name}. {ex.Message}"
                                , pluginConfiguration.Name, ex.Message);
                        }
                    }

                    if (validationMessage.MessageType == NotificationMessageType.Error)
                    {
                        model.Messages.Add(validationMessage);
                    }
                    else
                    {
                        if (!validConfigurationValues.Exists(c => c.PluginConfigurationId == existingConfigurationValue.PluginConfigurationId))
                        {
                            validConfigurationValues.Add(existingConfigurationValue);

                            // in the case of Multi-Select, _empty_ will always be sent and create a validation message. subsequent form values
                            // with the same name can make the configuration valid so we need to remove the existing validation message.
                            var controlId = GetControlId(pluginConfiguration, existingConfigurationValue);
                            model.Messages.RemoveAll(v => v.Detail == controlId);
                        }
                    }
                }

                if (model.Messages.Count > 0)
                {
                    return GetValidationErrorResponse(model.Messages);
                }

                foreach (ConfigurationValue configurationValue in validConfigurationValues)
                {
                    if (configurationValue.Id == 0)
                    {
                        DataStoreProvider.ConfigurationValueDataStore.Create(configurationValue);
                    }
                    else
                    {
                        DataStoreProvider.ConfigurationValueDataStore.Update(configurationValue);
                    }
                }
            } 
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
            }

            return new ObjectResult("Configuration Values saved.") { StatusCode = 200 };
        }

        private static NotificationMessage ValidateConfigurationValue(PluginConfiguration pluginConfiguration, ConfigurationValue configurationValue)
        {
            var notificationMessage = new NotificationMessage();
            var controlId = GetControlId(pluginConfiguration, configurationValue);

            if (pluginConfiguration.IsRequired && String.IsNullOrWhiteSpace(configurationValue.Value))
            {
                return new NotificationMessage()
                {
                    MessageType = NotificationMessageType.Error,
                    Message = $"{pluginConfiguration.Name} is required.",
                    Detail = controlId
                };
            }

            switch (pluginConfiguration.DataType)
            {
                case Public.ConfigurationDataType.Integer:
                    if (!int.TryParse(configurationValue.Value, out int valueInt)) { return GetErrorNotification(pluginConfiguration, valueInt.ToString(), controlId); }
                    break;
                case Public.ConfigurationDataType.Decimal:
                    if (!decimal.TryParse(configurationValue.Value, out decimal valueDec)) { return GetErrorNotification(pluginConfiguration, valueDec.ToString(), controlId); }
                    break;
                case Public.ConfigurationDataType.DateTime:
                    if (!DateTime.TryParse(configurationValue.Value, out DateTime valueDate)) { return GetErrorNotification(pluginConfiguration, valueDate.ToString(), controlId); }
                    break;
            }

            return notificationMessage;
        }

        private static NotificationMessage GetErrorNotification(PluginConfiguration pluginConfiguration, string value, string controlId)
        {
            return new NotificationMessage()
            {
                MessageType = NotificationMessageType.Error,
                Message = $"{pluginConfiguration.Name} value of {value} is not a valid {pluginConfiguration.DataType}.",
                Detail = controlId
            };
        }

        private static string GetControlId(PluginConfiguration pluginConfiguration, ConfigurationValue configurationValue)
        {
            var prefix = pluginConfiguration.DataType == Public.ConfigurationDataType.Boolean ? "configcheck-" : "config-";

            return $"{prefix}{pluginConfiguration.Id}-{configurationValue.Id}";
        }
    }
}

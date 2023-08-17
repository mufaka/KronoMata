using FluentValidation;
using FluentValidation.Results;
using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KronoMata.Web.Controllers
{
    public class ScheduledJobController : BaseController
    {
        private readonly ILogger<ScheduledJobController> _logger;
        private readonly IValidator _scheduledJobValidator;

        public ScheduledJobController(ILogger<ScheduledJobController> logger, IDataStoreProvider dataStoreProvider,
            IConfiguration configuration, IValidator<ScheduledJob> scheduledJobValidator)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
            _scheduledJobValidator = scheduledJobValidator;
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
                var jobs = DataStoreProvider.ScheduledJobDataStore.GetAll()
                    .OrderByDescending(s => s.IsEnabled)
                    .ThenBy(s => s.Name)
                    .ToList();

                return Json(jobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Host related data.");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }

        private void PopulateCommonProperties(ScheduledJobSaveViewModel model, bool updating)
        {
            var hosts = DataStoreProvider.HostDataStore.GetAll()
                .Where(h => h.IsEnabled || updating)
                .OrderBy(h => h.MachineName)
                .ToList();

            var allHost = new Model.Host()
            {
                Id = 0,
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
                    DataStoreProvider.ScheduledJobDataStore.Create(scheduledJob);
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

            return RedirectToAction("Index");
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
                        if (scheduledJob.HostId <= 0)
                        {
                            scheduledJob.HostId = null;
                        }

                        DataStoreProvider.ScheduledJobDataStore.Update(scheduledJob);
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

            return RedirectToAction("Index");
        }

        public ActionResult Configure(int id)
        {
            var model = new ConfigureScheduledJobViewModel();
            model.ViewName = "Scheduled Job Configuration";

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
                                existingConfigurationValue = new ConfigurationValue();
                                existingConfigurationValue.ScheduledJobId = id;
                                existingConfigurationValue.PluginConfigurationId = pluginConfiguration.Id;
                                existingConfigurationValue.InsertDate = DateTime.Now;
                                existingConfigurationValue.UpdateDate = existingConfigurationValue.InsertDate;
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
    }
}

using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    public class ScheduledJobController : BaseController
    {
        private readonly ILogger<ScheduledJobController> _logger;

        public ScheduledJobController(ILogger<ScheduledJobController> logger, IDataStoreProvider dataStoreProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
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
                ActionUrl = Url.Action("Create", "ScheduledJob")
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScheduledJobSaveViewModel model)
        {
            model.ViewName = "Scheduled Job Create";
#pragma warning disable CS8601 // Possible null reference assignment.
            model.ActionUrl = Url.Action("Create", "ScheduledJob");
#pragma warning restore CS8601 // Possible null reference assignment.

            try
            {
                PopulateCommonProperties(model, false);
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
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
            var model = new ScheduledJobSaveViewModel();

            model.ViewName = "Scheduled Job Update";
#pragma warning disable CS8601 // Possible null reference assignment.
            model.ActionUrl = Url.Action("Update", "ScheduledJob");
#pragma warning restore CS8601 // Possible null reference assignment.

            try
            {
                var existing = DataStoreProvider.ScheduledJobDataStore.GetById(scheduledJob.Id);

                if (existing != null)
                {
                    scheduledJob.InsertDate = existing.InsertDate;
                    scheduledJob.UpdateDate = DateTime.Now;

                    DataStoreProvider.ScheduledJobDataStore.Update(scheduledJob);
                }
                else
                {
                    _logger.LogError("Unable to get ScheduledJob with Id {id}", scheduledJob.Id);
                    return new ObjectResult($"Unable to get ScheduledJob with Id {scheduledJob.Id}") { StatusCode = 500 };
                }

                PopulateCommonProperties(model, true);
                model.ScheduledJob = scheduledJob;
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
                return View("Save", model);
            }

            return RedirectToAction("Index");
        }

    }
}

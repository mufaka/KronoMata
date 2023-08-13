using KronoMata.Data;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    public class HostController : BaseController
    {
        private readonly ILogger<HostController> _logger;

        public HostController(ILogger<HostController> logger, IDataStoreProvider dataStoreProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = new HostViewModel()
            {
                ViewName = "Hosts"
            };

            try
            {
                model.Hosts = DataStoreProvider.HostDataStore.GetAll()
                    .OrderBy(h => h.IsEnabled)
                    .ThenBy(h => h.MachineName).ToList();
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
            }

            return View(model);
        }

        public ActionResult GetHostRelatedData(int hostId)
        {
            try
            {
                var host = DataStoreProvider.HostDataStore.GetById(hostId);
                var jobs = DataStoreProvider.ScheduledJobDataStore.GetByHost(hostId);

                var db = Json(new
                {
                    host,
                    jobs
                });

                return Json(new
                {
                    data = db
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Host related data.");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }

        [HttpPost]
        public ActionResult SaveHost(Model.Host host)
        {
            try
            {
                var now = DateTime.Now;

                host.UpdateDate = now;

                if (host.Id <= 0)
                {
                    host.InsertDate = now;
                    DataStoreProvider.HostDataStore.Create(host);
                } 
                else
                {
                    DataStoreProvider.HostDataStore.Update(host);
                }

                DataStoreProvider.HostDataStore.Update(host);

                return Json(new { host });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Host related data.");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }
    }
}

using KronoMata.Data;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KronoMata.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IDataStoreProvider dataStoreProvider)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel();
            model.ViewName = "Dashboard";

            try
            {
                var now = DateTime.Now.Date;

                model.Plugins = DataStoreProvider.PluginMetaDataDataStore.GetAll();
                model.Hosts = DataStoreProvider.HostDataStore.GetAll();
                model.ScheduledJobs = DataStoreProvider.ScheduledJobDataStore.GetAll();
                model.JobHistories = DataStoreProvider.JobHistoryDataStore.GetLastByDate(now.AddDays(-7));
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, ex.Message);
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
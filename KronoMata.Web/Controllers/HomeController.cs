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

            try
            {
                var now = DateTime.Now.Date;

                model.Plugins = DataStoreProvider.PluginMetaDataDataStore.GetAll();
                model.Hosts = DataStoreProvider.HostDataStore.GetAll();
                model.ScheduledJobs = DataStoreProvider.ScheduledJobDataStore.GetAll();
                model.JobHistories = DataStoreProvider.JobHistoryDataStore.GetLastByDate(now.AddDays(-7));

                LogMessage(model, NotificationMessageType.Error, "Test Error", "This is only a test");
                LogMessage(model, NotificationMessageType.Exception, "Test Exception", "This is only a test");
                LogMessage(model, NotificationMessageType.Information, "Test Information", "This is only a test");
                LogMessage(model, NotificationMessageType.Success, "Test Success", "This is only a test");
                LogMessage(model, NotificationMessageType.Warning, "Test Warning", "This is only a test");
            }
            catch (Exception ex)
            {
                LogException(model, ex);
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
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
    }
}

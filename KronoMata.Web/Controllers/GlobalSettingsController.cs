using KronoMata.Data;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    public class GlobalSettingsController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public GlobalSettingsController(ILogger<HomeController> logger, IDataStoreProvider dataStoreProvider)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
        }

        public IActionResult Index()
        {
            var model = new GlobalSettingsViewModel();

            try
            {
                model.GlobalConfigurations = DataStoreProvider.GlobalConfigurationDataStore.GetAll();
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, ex.Message);
            }

            return View(model);
        }
    }
}

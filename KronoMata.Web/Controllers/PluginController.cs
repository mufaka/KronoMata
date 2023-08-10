using KronoMata.Data;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    public class PluginController : BaseController
    {
        private ILogger<PluginController> _logger;

        public PluginController(ILogger<PluginController> logger, IDataStoreProvider dataStoreProvider, 
            IConfiguration configuration)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = new PluginViewModel();

            try
            {
                model.Plugins = DataStoreProvider.PluginMetaDataDataStore.GetAll()
                    .OrderBy(p => p.Name)
                    .ToList();
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

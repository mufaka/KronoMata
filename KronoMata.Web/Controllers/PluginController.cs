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
            model.ViewName = "Plugins";

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

        public ActionResult GetPluginParameterData(int pageIndex, int pageSize, int plugin)
        {
            try
            {
                var parameters = DataStoreProvider.PluginConfigurationDataStore.GetByPluginMetaData(plugin);

                var result = Json(new
                {
                    data = parameters,
                    itemsCount = parameters.Count
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }
    }
}

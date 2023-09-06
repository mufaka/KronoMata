using KronoMata.Data;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    [Authorize]
    public class PluginController : BaseController
    {
        private readonly ILogger<PluginController> _logger;

        public PluginController(ILogger<PluginController> logger, IDataStoreProvider dataStoreProvider, 
            IConfiguration configuration)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = new PluginViewModel
            {
                ViewName = "Plugins"
            };

            try
            {
                model.Plugins = DataStoreProvider.PluginMetaDataDataStore.GetAll()
                    .OrderBy(p => p.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
            }

            return View(model);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public ActionResult GetPluginParameterData(int pageIndex, int pageSize, int plugin)
#pragma warning restore IDE0060 // Remove unused parameter
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
                _logger.LogError(ex, "Error getting PluginConfiguration data.");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }
    }
}

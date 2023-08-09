using KronoMata.Data;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    public class PackageController : BaseController
    {
        private readonly ILogger<JobHistoryController> _logger;

        public PackageController(ILogger<JobHistoryController> logger, IDataStoreProvider dataStoreProvider, 
            IConfiguration configuration)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = new PackageViewModel();
            model.ViewName = "Packages";

            try
            {
                model.Packages = DataStoreProvider.PackageDataStore.GetAll();
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

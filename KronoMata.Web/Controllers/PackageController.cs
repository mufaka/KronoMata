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
                model.Packages = DataStoreProvider.PackageDataStore.GetAll()
                    .OrderBy(x => x.Name).ToList(); ;
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, ex.Message);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string packageName, IFormFile file)
        {
            var model = new PackageViewModel();
            model.ViewName = "Packages";

            try
            {
                // lets see what we get here and when ....

                model.Packages = DataStoreProvider.PackageDataStore.GetAll()
                    .OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, ex.Message);

                return View(model);
            }

            return RedirectToAction("Index", "Package");
        }
    }
}

using KronoMata.Data;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    public class JobHistoryController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public JobHistoryController(ILogger<HomeController> logger, IDataStoreProvider dataStoreProvider)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
        }

        public IActionResult Index()
        {
            var model = new JobHistoryViewModel();
            model.ViewName = "Job History";

            return View(model);
        }

        public ActionResult GetJobHistoryData(int pageIndex, int pageSize)
        {
            // jsGrid sends a parameter named pageIndex but it is a 1 based index.
            var pagedList = DataStoreProvider.JobHistoryDataStore.GetAllPaged(pageIndex - 1, pageSize);

            var histories = pagedList.List;

            var result = Json(new
            {
                data = histories,
                itemsCount = pagedList.TotalRecords
            });

            return result;
        }
    }
}

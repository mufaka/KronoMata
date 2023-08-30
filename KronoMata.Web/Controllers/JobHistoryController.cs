using KronoMata.Data;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    public class JobHistoryController : BaseController
    {
        private readonly ILogger<JobHistoryController> _logger;

        public JobHistoryController(ILogger<JobHistoryController> logger, IDataStoreProvider dataStoreProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = new JobHistoryViewModel
            {
                ViewName = "Job History"
            };

            return View(model);
        }

        public ActionResult GetJobHistoryData(int pageIndex, int pageSize, int status = -1, int scheduledJobId = -1, int hostId = -1)
        {
            try
            {
                // jsGrid sends a parameter named pageIndex but it is a 1 based index.
                var pagedList = DataStoreProvider.JobHistoryDataStore.GetFilteredPaged(pageIndex - 1, pageSize, status, scheduledJobId, hostId);

                var result = Json(new
                {
                    data = pagedList.List,
                    itemsCount = pagedList.TotalRecords
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting JobHistory data.");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }

        public int Expire()
        {
            try
            {
                var maxDays = int.TryParse(DataStoreProvider.GlobalConfigurationDataStore.
                    GetByCategoryAndName("JobHistory", "MaxDays").Value, out int expiration)
                    ? expiration
                : 14;

                var maxRecords = int.TryParse(DataStoreProvider.GlobalConfigurationDataStore.
                    GetByCategoryAndName("JobHistory", "MaxRecords").Value, out int max)
                    ? max
                    : 10000;

                var expiredRecordCount = DataStoreProvider.JobHistoryDataStore.Expire(maxDays, maxRecords);

                _logger.LogInformation("Expired {expiredRecordCount} Job History records.", expiredRecordCount);

                return expiredRecordCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error expiring Job History.");
            }

            return 0;
        }
    }
}

using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    [Authorize]
    public class ChartDataController : BaseController
    {
        private readonly ILogger<ChartDataController> _logger;

        public ChartDataController(ILogger<ChartDataController> logger, IDataStoreProvider dataStoreProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
        }

        public ActionResult GetLastJobHistorySummary()
        {
            try
            {
                // TODO: accept a start date parameter
                var now = DateTime.Now;
                var startDate = now.Date.AddDays(-7);
                var histories = DataStoreProvider.JobHistoryDataStore.GetLastByDate(startDate);

                // get the dates in the range
                var dates = Enumerable.Range(0, 1 + now.Subtract(startDate).Days)
                        .Select(offset => startDate.AddDays(offset).Date)
                        .ToList();

                // get a list of days in the form of <month>-<day> (0 pad)
                var dateLabels = dates.Select(d => $"{d.Month.ToString().PadLeft(2, '0')}-{d.Day.ToString().PadLeft(2, '0')}").ToArray();

                // split into buckets for Success, Fail, Skipped (job history status enum)
                var dataSeriesSet = new Dictionary<string, ChartDataSeries<int>>();
                var dataSeriesList = new List<ChartDataSeries<int>>();

                foreach (JobHistory history in histories)
                {
                    var seriesName = history.Status.ToString();

                    if (!dataSeriesSet.ContainsKey(seriesName))
                    {
                        var dataSeries = new ChartDataSeries<int>()
                        {
                            Name = seriesName,
                            Data = new List<int>(new int[dates.Count]) // initialize date buckets to zero
                        };

                        dataSeriesSet.Add(seriesName, dataSeries);
                        dataSeriesList.Add(dataSeries);
                    }

                    // where does this history's date fall in the bucket?
                    var span = now.Date - history.RunTime.Date;
                    var bucketIndex = (dates.Count - span.Days) - 1;

                    // add to the count in the bucket
                    dataSeriesSet[seriesName].Data[bucketIndex] += 1;
                }

                var result = Json(new
                {
                    labels = dateLabels,
                    seriesList = dataSeriesList
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting last JobHistory");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }

        public ActionResult GetLastJobHistoryDetail(int pageIndex, int pageSize)
        {
            try
            {
                // TODO: accept a start date parameter
                var now = DateTime.Now;
                var startDate = now.Date.AddDays(-7);

                // jsGrid sends a parameter named pageIndex but it is a 1 based index.
                var pagedList = DataStoreProvider.JobHistoryDataStore.GetLastByDatePaged(startDate, pageIndex - 1, pageSize);

                var histories = pagedList.List;

                var result = Json(new
                {
                    data = histories,
                    itemsCount = pagedList.TotalRecords
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting JobHistory details.");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }

        public ActionResult GetLastJobHistoryRelatedData()
        {
            try
            {
                var hosts = DataStoreProvider.HostDataStore.GetAll();
                var plugins = DataStoreProvider.PluginMetaDataDataStore.GetAll();
                var jobs = DataStoreProvider.ScheduledJobDataStore.GetAll();

                var db = Json(new
                {
                    hosts,
                    plugins,
                    jobs
                });

                return Json(new
                {
                    data = db
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting JobHistory related data.");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }
    }
}

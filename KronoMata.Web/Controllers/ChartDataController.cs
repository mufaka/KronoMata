using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    public class ChartDataController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public ChartDataController(ILogger<HomeController> logger, IDataStoreProvider dataStoreProvider)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
        }

        public ActionResult GetLastJobHistorySummary()
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

                // where does this histories date fall in the bucket?
                var span = now - history.RunTime;
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

        public ActionResult GetLastJobHistoryDetail()
        {
            // TODO: accept a start date parameter
            var now = DateTime.Now;
            var startDate = now.Date.AddDays(-7);
            var histories = DataStoreProvider.JobHistoryDataStore.GetLastByDate(startDate);
            var hosts = DataStoreProvider.HostDataStore.GetAll();
            var plugins = DataStoreProvider.PluginMetaDataDataStore.GetAll();
            var jobs = DataStoreProvider.ScheduledJobDataStore.GetAll();

            var result = Json(new
            {
                histories = histories,
                hosts = hosts,
                plugins = plugins,
                jobs = jobs
            });

            return result;
        }

    }
}

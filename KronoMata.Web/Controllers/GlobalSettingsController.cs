using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace KronoMata.Web.Controllers
{
    public class GlobalSettingsController : BaseController
    {
        private readonly ILogger<GlobalSettingsController> _logger;

        public GlobalSettingsController(ILogger<GlobalSettingsController> logger, IDataStoreProvider dataStoreProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = new GlobalSettingsViewModel
            {
                ViewName = "Settings"
            };

            try
            {
                model.ExpirationDays = int.TryParse(DataStoreProvider.GlobalConfigurationDataStore.
                    GetByCategoryAndName("JobHistory", "MaxDays").Value, out int expiration) 
                    ? expiration 
                    : 14;

                model.MaximumHistoryRecords = int.TryParse(DataStoreProvider.GlobalConfigurationDataStore.
                    GetByCategoryAndName("JobHistory", "MaxRecords").Value, out int max) 
                    ? max 
                    : 10000;

                var jobHistoryTableStat = DataStoreProvider.JobHistoryDataStore.GetTableStat();
                model.JobHistoryCount = jobHistoryTableStat.RowCount;
                model.OldestHistoryDate = jobHistoryTableStat.OldestRecord.HasValue ? jobHistoryTableStat.OldestRecord.Value : DateTime.MinValue;
                model.NewestHistoryDate = jobHistoryTableStat.NewestRecord.HasValue ? jobHistoryTableStat.NewestRecord.Value : DateTime.MinValue;
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
            }

            return View(model);
        }

        public ActionResult GetGlobalSettingsData()
        {
            try
            {
                var settings = DataStoreProvider.GlobalConfigurationDataStore.GetAll();

                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var result = Json(settings, serializerOptions);

                return result;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting GlobalConfiguration data.");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }

        [HttpPost]
        public void SaveGlobalSettings(GlobalConfiguration data)
        {
            try
            {
                DataStoreProvider.GlobalConfigurationDataStore.Create(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving GlobalConfiguration data.");
            }
        }

        [HttpPost]
        public void UpdateGlobalSettings(GlobalConfiguration data)
        {
            try
            {
                var existing = DataStoreProvider.GlobalConfigurationDataStore.GetById(data.Id);

                if (existing != null)
                {
                    // don't allow changing the names of system configuration
                    if (existing.IsSystemConfiguration)
                    {
                        data.Category = existing.Category;
                        data.Name = existing.Name;
                    }

                    data.InsertDate = existing.InsertDate;
                    data.UpdateDate = DateTime.Now;

                    DataStoreProvider.GlobalConfigurationDataStore.Update(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating GlobalConfiguration data.");
            }
        }

        [HttpPost]
        public void DeleteGlobalSettings(GlobalConfiguration data)
        {
            try
            {
                DataStoreProvider.GlobalConfigurationDataStore.Delete(data.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting GlobalConfiguration data.");
            }
        }

    }
}

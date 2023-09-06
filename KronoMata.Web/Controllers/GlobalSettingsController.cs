using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace KronoMata.Web.Controllers
{
    [Authorize]
    public class GlobalSettingsController : BaseController
    {
        private readonly ILogger<GlobalSettingsController> _logger;
        private readonly IDataProtector _dataProtector;

        public GlobalSettingsController(ILogger<GlobalSettingsController> logger, IDataStoreProvider dataStoreProvider,
            IConfiguration configuration, IDataProtectionProvider dataProtectionProvider)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
            _dataProtector = dataProtectionProvider.CreateProtector("KronoMata.Web.v1");
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

                foreach (GlobalConfiguration globalConfiguration in settings)
                {
                    if (globalConfiguration.IsMasked)
                    {
                        // bootstrapping / existing database compatability. Ignore
                        // decryption errors but log warnings (without values!)
                        try
                        {
                            globalConfiguration.Value = _dataProtector.Unprotect(globalConfiguration.Value);
                        }
                        catch (Exception ex) 
                        {
                            _logger.LogWarning("Unable to decrypt Global Configuration Value for Category {globalConfiguration.Category}, Name {globalConfiguration.Name}. {ex.Message}"
                                , globalConfiguration.Category, globalConfiguration.Name, ex.Message);
                        }
                    }
                }

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

        private void EncryptGlobalConfigurationValue(GlobalConfiguration globalConfiguration)
        {
            if (globalConfiguration.IsMasked)
            {
                try
                {
                    globalConfiguration.Value = _dataProtector.Protect(globalConfiguration.Value);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Unable to encrypt Global Configuration Value for Category {globalConfiguration.Category}, Name {globalConfiguration.Name}. {ex.Message}"
                        , globalConfiguration.Category, globalConfiguration.Name, ex.Message);
                }
            }
        }

        [HttpPost]
        public void SaveGlobalSettings(GlobalConfiguration data)
        {
            try
            {
                EncryptGlobalConfigurationValue(data);
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

                    EncryptGlobalConfigurationValue(data);
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

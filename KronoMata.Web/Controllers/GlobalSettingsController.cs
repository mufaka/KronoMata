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

        public GlobalSettingsController(ILogger<GlobalSettingsController> logger, IDataStoreProvider dataStoreProvider)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
        }

        public IActionResult Index()
        {
            var model = new GlobalSettingsViewModel();
            model.ViewName = "Settings";

            try
            {
                // using ajax to get the data
                // model.GlobalConfigurations = DataStoreProvider.GlobalConfigurationDataStore.GetAll();
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, ex.Message);
            }

            return View(model);
        }

        public ActionResult GetGlobalSettingsData()
        {
            var settings = DataStoreProvider.GlobalConfigurationDataStore.GetAll();

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.PropertyNameCaseInsensitive = true;
            var result = Json(settings, serializerOptions);

            return result;
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
                _logger.LogError(ex, ex.Message);
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
                    data.InsertDate = existing.InsertDate;
                    data.UpdateDate = DateTime.Now;

                    DataStoreProvider.GlobalConfigurationDataStore.Update(data);
                }
                else
                {
                    // TODO: Should we create another one? Probably not.
                    // TODO: return error
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                _logger.LogError(ex, ex.Message);
            }
        }

    }
}

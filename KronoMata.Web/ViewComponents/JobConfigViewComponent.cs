using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.ViewComponents
{
    /// <summary>
    /// Renders form controls based on the PluginConfiguration
    /// DataType.
    /// </summary>
    public class JobConfigViewComponent : ViewComponent
    {
        private readonly IDataStoreProvider _dataStoreProvider;

        public JobConfigViewComponent(IDataStoreProvider dataStoreProvider)
        {
            _dataStoreProvider = dataStoreProvider;
        }

        public IViewComponentResult Invoke(ConfigurationValue configurationValue)
        {
            var pluginConfiguration = _dataStoreProvider.PluginConfigurationDataStore.GetById(configurationValue.PluginConfigurationId);
            var model = new JobConfigViewModel(pluginConfiguration, configurationValue);


            // switch the 'View' based on the PluginConfiguration DataType.
            var view = "ConfigTextBox";

            switch (pluginConfiguration.DataType)
            {
                case Public.ConfigurationDataType.Boolean:
                    view = "ConfigCheckBox";
                    break;
                case Public.ConfigurationDataType.DateTime:
                    view = "ConfigDateTime";
                    break;
                case Public.ConfigurationDataType.Decimal:
                case Public.ConfigurationDataType.Integer:
                    view = "ConfigNumeric";
                    break;
                case Public.ConfigurationDataType.Password:
                    view = "ConfigPassword";
                    break;
                case Public.ConfigurationDataType.String:
                    view = "ConfigTextBox";
                    break;
                case Public.ConfigurationDataType.Text:
                    view = "ConfigTextArea";
                    break;
                case Public.ConfigurationDataType.Select:
                    view = "ConfigSelect";
                    break;
                case Public.ConfigurationDataType.SelectMultiple:
                    view = "ConfigSelectMultiple";
                    break;
            }

            return View(view, model);
        }
    }
}

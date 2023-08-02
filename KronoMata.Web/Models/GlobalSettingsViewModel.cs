using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class GlobalSettingsViewModel : BaseViewModel
    {
        public List<GlobalConfiguration> GlobalConfigurations { get; set; } = new List<GlobalConfiguration>();
    }
}

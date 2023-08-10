using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class PluginViewModel : BaseViewModel
    {
        public List<PluginMetaData> Plugins { get; set; } = new List<PluginMetaData>();
    }
}

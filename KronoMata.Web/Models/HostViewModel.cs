using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class HostViewModel : BaseViewModel
    {
        public List<Model.Host> Hosts { get; set; } = new List<Model.Host>();
        public int WarningMinutesThreshold { get; set; } = 5;
        public string GetHostBgClass(Model.Host host)
        {
            if (!host.IsEnabled)
            {
                return "bg-gray";
            }

            var minutesSinceLastContact = (int)(Now - host.UpdateDate).Duration().TotalMinutes;

            return minutesSinceLastContact >= WarningMinutesThreshold ? "bg-warning" : "bg-info";
        }
    }
}

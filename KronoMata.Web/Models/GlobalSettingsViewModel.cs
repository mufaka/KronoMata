using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class GlobalSettingsViewModel : BaseViewModel
    {
        public List<GlobalConfiguration> GlobalConfigurations { get; set; } = new List<GlobalConfiguration>();
        public int JobHistoryCount { get; set; }
        public DateTime OldestHistoryDate { get; set; } = DateTime.Now;
        public DateTime NewestHistoryDate { get; set; } = DateTime.Now;
        public int ExpirationDays { get; set; }
        public int MaximumHistoryRecords { get; set; }
    }
}

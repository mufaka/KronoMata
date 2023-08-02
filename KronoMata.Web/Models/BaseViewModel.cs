using KronoMata.Data;

namespace KronoMata.Web.Models
{
    public abstract class BaseViewModel
    {
        public BaseViewModel()
        {
            Messages = new List<NotificationMessage>();
        }

        public string ViewName { get; set; } = "KronoMata View";

        public List<NotificationMessage> Messages { get; set; }

        public DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}

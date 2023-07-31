using KronoMata.Data;

namespace KronoMata.Web.Models
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            Messages = new List<NotificationMessage>();
        }

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

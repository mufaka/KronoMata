namespace KronoMata.Web.Models
{
    public class NotificationMessage
    {
        public NotificationMessageType MessageType { get; set; }
        public string Message { get; set; } = String.Empty;
        public string Detail { get; set; } = String.Empty;
    }
}

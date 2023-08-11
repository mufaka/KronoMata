using KronoMata.Data;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace KronoMata.Web.Controllers
{
    public class BaseController : Controller
    {
        // these properties are set by derived classes
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public IDataStoreProvider DataStoreProvider { get; protected set; }
        public IConfiguration Configuration { get; protected set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public void LogMessage(BaseViewModel model, NotificationMessageType messageType, 
            string message, string detail)
        {
            model.Messages.Add(new NotificationMessage()
            {
                MessageType = messageType,
                Message = message,
                Detail = detail
            });
        }

        public void LogException(BaseViewModel model, Exception ex)
        {
            LogMessage(model, NotificationMessageType.Exception, ex.Message, ex.ToString());
        }
    }
}

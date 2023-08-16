using FluentValidation.Results;
using KronoMata.Data;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

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

        protected List<NotificationMessage> GetNotificationsFromValidationResult(ValidationResult validationResult)
        {
            var result = new List<NotificationMessage>();

            foreach (ValidationFailure validationFailure in validationResult.Errors)
            {
                result.Add(new NotificationMessage()
                {
                    MessageType = NotificationMessageType.Error,
                    Message = validationFailure.ErrorMessage,
                    Detail = validationFailure.PropertyName
                });
            }

            return result;
        }

        protected ActionResult GetValidationErrorResponse(ValidationResult validationResult)
        {
            return GetValidationErrorResponse(GetNotificationsFromValidationResult(validationResult));
        }

        protected ActionResult GetValidationErrorResponse(BaseViewModel model)
        {
            return GetValidationErrorResponse(model.Messages);
        }

        protected ActionResult GetValidationErrorResponse(List<NotificationMessage> messages)
        {
            var validationJson = JsonSerializer.Serialize(messages);

            // status code 422 is 'Unprocessable Entity'. Caller should expect response message to be validation errors.
            // TODO: Is there a max length for the status code message?
            return new ObjectResult(validationJson) { StatusCode = 422 };
        }
    }
}

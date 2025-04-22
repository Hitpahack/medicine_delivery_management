using Microsoft.Extensions.Localization;

namespace RepMed.Localize
{
    public interface IValidationMessagesServices
    {
        public string Success { get; }
        public string InternalError { get; }
        public string AddSuccess { get; }
        public string GetAddSuccess(string name);
        public string RetriveSuccess { get; }
        public string GetRetriveSuccess(string name);
        public string UpdateSuccess { get; }
        public string GetUpdateSuccess(string name);
        public string AlreadyExist { get; }
        public string NotExist { get; }
        public string EmailNotConfirm { get; }
        public string AcNotActive { get; }
        public string InvalidPassword { get; }
        public string InvalidToken { get; }
        public string TokenExpired { get; }
        public string GetAlreadyExist(string name);
        public string GetNotExist(string name);
        public string GetAlreadyExist(string name, params object[] parma);
        public string CreatedByNotExist { get; }


    }
    public class ValidationMessagesServices : IValidationMessagesServices
    {
        public readonly IStringLocalizer<ValidationMessagesResource> _localizer;
        public ValidationMessagesServices(IStringLocalizer<ValidationMessagesResource> localizer)
        {
            _localizer = localizer;
        }

        string IValidationMessagesServices.AddSuccess => _localizer["Data successfully added."];
        string IValidationMessagesServices.CreatedByNotExist => _localizer["CreatedBy id not exist in record."];
        string IValidationMessagesServices.TokenExpired => _localizer["Either this url is expired or invalid, please request for a new one."];
        string IValidationMessagesServices.InternalError => _localizer["Something wrong internal error."];
        string IValidationMessagesServices.Success => _localizer["Request successfully processed."];
        string IValidationMessagesServices.RetriveSuccess => _localizer["Data successfully retrive."];

        string IValidationMessagesServices.UpdateSuccess => _localizer["Data successfully updated."];

        string IValidationMessagesServices.AlreadyExist => _localizer["Requested value already exist."];
        string IValidationMessagesServices.NotExist => _localizer["value doesn't exist."];
        string IValidationMessagesServices.EmailNotConfirm => _localizer["Email not confirm, please confirm email address."];
        string IValidationMessagesServices.AcNotActive => _localizer["Account not activated."];
        string IValidationMessagesServices.InvalidPassword => _localizer["You have entered invalid password."];
        string IValidationMessagesServices.InvalidToken => _localizer["You have entered invalid token."];

        public string GetAddSuccess(string name)
        {
            return _localizer[name+" data successfully added"];
        }
        public string GetRetriveSuccess(string name)
        {
            return _localizer[name+" data successfully retrive"];
        }

        public string GetAlreadyExist(string name)
        {
            return _localizer[name+" already exist"];
        }
        public string GetNotExist(string name)
        {
            return _localizer[name + " doesn't exist"];
        }
        public string GetUpdateSuccess(string name)
        {
            return _localizer[name+" data successfully updated"];
        }
        public string GetAlreadyExist(string name, params object[] parma)
        {
            return _localizer[name + " already exist, {0}", parma];
        }
    }
}

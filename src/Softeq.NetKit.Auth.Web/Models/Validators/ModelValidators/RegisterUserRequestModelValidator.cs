// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Web.Models.Request;

namespace Softeq.NetKit.Auth.Web.Models.Validators.ModelValidators
{
    public class RegisterUserRequestModelValidator : BaseValidatorInterceptor<RegisterUserRequestModel>
    {
        public RegisterUserRequestModelValidator(EmailValidator emailValidator, PasswordValidator passwordValidator)
        {
            Include(emailValidator);
            Include(passwordValidator);
        }
    }
}
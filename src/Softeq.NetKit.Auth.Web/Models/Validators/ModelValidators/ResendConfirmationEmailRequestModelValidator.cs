// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Web.Models.Request;

namespace Softeq.NetKit.Auth.Web.Models.Validators.ModelValidators
{
    public class ResendConfirmationEmailRequestModelValidator : BaseValidatorInterceptor<ResendConfirmationEmailRequestModel>
    {
        public ResendConfirmationEmailRequestModelValidator(EmailValidator emailValidator)
        {
            Include(emailValidator);
        }
    }
}
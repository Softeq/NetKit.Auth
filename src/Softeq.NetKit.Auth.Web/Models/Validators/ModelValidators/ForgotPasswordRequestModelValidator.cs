// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Web.Models.Request;

namespace Softeq.NetKit.Auth.Web.Models.Validators.ModelValidators
{
    public class ForgotPasswordRequestModelValidator : BaseValidatorInterceptor<ForgotPasswordRequestModel>
    {
        public ForgotPasswordRequestModelValidator(EmailValidator emailValidator)
        {
            Include(emailValidator);
        }
    }
}
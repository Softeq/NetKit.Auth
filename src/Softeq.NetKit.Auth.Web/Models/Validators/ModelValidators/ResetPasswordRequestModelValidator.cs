// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Web.Models.Request;

namespace Softeq.NetKit.Auth.Web.Models.Validators.ModelValidators
{
    public class ResetPasswordRequestModelValidator : BaseValidatorInterceptor<ResetPasswordRequestModel>
    {
        public ResetPasswordRequestModelValidator(PasswordValidator passwordValidator, ConfirmPasswordValidator confirmPasswordValidator, CodeValidator codeValidator)
        {
            Include(passwordValidator);
            Include(confirmPasswordValidator);
            Include(codeValidator);
        }
    }
}
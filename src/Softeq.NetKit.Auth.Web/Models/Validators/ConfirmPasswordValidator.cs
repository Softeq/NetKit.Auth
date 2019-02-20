// Developed by Softeq Development Corporation
// http://www.softeq.com

using FluentValidation;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Web.Models.Request;

namespace Softeq.NetKit.Auth.Web.Models.Validators
{
    public class ConfirmPasswordValidator : AbstractValidator<IConfirmPasswordModel>
    {
        public ConfirmPasswordValidator()
        {
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithErrorCode(ValidationErrorCode.ConfirmPasswordDoesNotMatch).WithMessage("Passwords don't match.");
        }
    }
}
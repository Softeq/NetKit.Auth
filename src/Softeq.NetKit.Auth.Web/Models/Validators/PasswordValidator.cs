// Developed by Softeq Development Corporation
// http://www.softeq.com

using FluentValidation;
using Softeq.NetKit.Auth.AppServices.Utility;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Web.Models.Request;

namespace Softeq.NetKit.Auth.Web.Models.Validators
{
    public class PasswordValidator : AbstractValidator<IPasswordModel>
    {
        public PasswordValidator(UserPasswordConfiguration configuration)
        {
            RuleFor(x => x.Password).NotNull().NotEmpty().WithErrorCode(ValidationErrorCode.FieldIsEmpty);
            RuleFor(x => x.Password).MinimumLength(configuration.RequiredLength).WithErrorCode(ValidationErrorCode.MinLengthRequired);
            RuleFor(x => x.Password).MaximumLength(configuration.MaximumLength).WithErrorCode(ValidationErrorCode.MaxLengthExceeded);
            RuleFor(x => x.Password).Matches(configuration.Regex).WithErrorCode(ValidationErrorCode.WeakPassword);
        }
    }
}
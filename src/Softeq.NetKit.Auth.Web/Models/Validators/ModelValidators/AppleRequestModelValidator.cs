// Developed by Softeq Development Corporation
// http://www.softeq.com

using FluentValidation;
using Softeq.NetKit.Auth.AppServices.Utility;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Web.Models.Request;

namespace Softeq.NetKit.Auth.Web.Models.Validators.ModelValidators
{
    public class AppleRequestModelValidator : BaseValidatorInterceptor<AppleRequestModel>
    {
        public AppleRequestModelValidator(UserEmailConfiguration configuration)
        {
            RuleFor(x => x.AppleKey).NotNull().NotEmpty().WithErrorCode(ValidationErrorCode.FieldIsEmpty);

            RuleFor(x => x.Email).MaximumLength(configuration.MaximumLength).WithErrorCode(ValidationErrorCode.MaxLengthExceeded);
            RuleFor(x => x.Email).EmailAddress().WithErrorCode(ValidationErrorCode.InvalidEmail);
        }
    }
}

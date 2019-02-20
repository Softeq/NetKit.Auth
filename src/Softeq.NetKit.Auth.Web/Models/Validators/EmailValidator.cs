// Developed by Softeq Development Corporation
// http://www.softeq.com

using FluentValidation;
using Softeq.NetKit.Auth.AppServices.Utility;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Web.Models.Request;

namespace Softeq.NetKit.Auth.Web.Models.Validators
{
    public class EmailValidator : AbstractValidator<IEmailModel>
    {
        public EmailValidator(UserEmailConfiguration configuration)
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().WithErrorCode(ValidationErrorCode.FieldIsEmpty);
            RuleFor(x => x.Email).MaximumLength(configuration.MaximumLength).WithErrorCode(ValidationErrorCode.MaxLengthExceeded);
            RuleFor(x => x.Email).EmailAddress().WithErrorCode(ValidationErrorCode.InvalidEmail);
        }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

using FluentValidation;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Web.Models.Request;

namespace Softeq.NetKit.Auth.Web.Models.Validators.ModelValidators
{
    public class GenerateTokenRequestModelValidator : BaseValidatorInterceptor<GenerateTokenRequestModel>
    {
        public GenerateTokenRequestModelValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty().WithErrorCode(ValidationErrorCode.FieldIsEmpty);
            RuleFor(x => x.Password).NotNull().NotEmpty().WithErrorCode(ValidationErrorCode.FieldIsEmpty);
            RuleFor(x => x.Scope).NotNull().NotEmpty().WithErrorCode(ValidationErrorCode.FieldIsEmpty);
            RuleFor(x => x.Grant_type).NotNull().NotEmpty().WithErrorCode(ValidationErrorCode.FieldIsEmpty);
            RuleFor(x => x.Client_id).NotNull().NotEmpty().WithErrorCode(ValidationErrorCode.FieldIsEmpty);
            RuleFor(x => x.Client_secret).NotNull().NotEmpty().WithErrorCode(ValidationErrorCode.FieldIsEmpty);
        }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

using FluentValidation;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Web.Models.Request;

namespace Softeq.NetKit.Auth.Web.Models.Validators
{
    public class CodeValidator : AbstractValidator<ICodeModel>
    {
        public CodeValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().WithErrorCode(ValidationErrorCode.FieldIsEmpty);
        }
    }
}
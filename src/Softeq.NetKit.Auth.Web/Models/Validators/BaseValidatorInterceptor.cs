// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Softeq.NetKit.Auth.Common.Exceptions.Exceptions;
using Softeq.NetKit.Auth.Common.Exceptions.Response;
using Microsoft.AspNetCore.Mvc;

namespace Softeq.NetKit.Auth.Web.Models.Validators
{
    public abstract class BaseValidatorInterceptor<TRequestModel> : AbstractValidator<TRequestModel>, IValidatorInterceptor
    {
        public ValidationContext BeforeMvcValidation(ControllerContext controllerContext, ValidationContext validationContext)
        {
            return validationContext;
        }

        public ValidationResult AfterMvcValidation(ControllerContext controllerContext, ValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => new ValidationError(error.PropertyName, error.ErrorCode, error.ErrorMessage)).ToList();
                throw new NetKitAuthValidationException(errors, "validation failed");
            }

            return result;
        }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.NetKit.Auth.Common.Exceptions.Response
{
    public class ValidationErrorResponseModel : ErrorResponseModel
    {
        public ValidationErrorResponseModel(ErrorCode errorCode, string message, List<ValidationError> validationErrors)
            : base(errorCode, message)
        {
            ValidationErrors = validationErrors;
        }

        public List<ValidationError> ValidationErrors { get; }
    }
}
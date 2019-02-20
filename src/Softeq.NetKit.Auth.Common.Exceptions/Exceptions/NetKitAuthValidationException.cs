// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.NetKit.Auth.Common.Exceptions.Response;

namespace Softeq.NetKit.Auth.Common.Exceptions.Exceptions
{
    public class NetKitAuthValidationException : NetKitAuthException
    {
        public NetKitAuthValidationException(List<ValidationError> errors)
            : base(ErrorCode.RequestModelValidationFailed)
        {
            Errors = errors;
        }

        public NetKitAuthValidationException(List<ValidationError> errors, string message)
            : base(ErrorCode.RequestModelValidationFailed, message)
        {
            Errors = errors;
        }

        public NetKitAuthValidationException(List<ValidationError> errors, string message, Exception innerException)
            : base(ErrorCode.RequestModelValidationFailed, message, innerException)
        {
            Errors = errors;
        }

        public List<ValidationError> Errors { get; }
    }
}
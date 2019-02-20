// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.NetKit.Auth.Common.Exceptions.Response
{
    public class IdentityErrorResponseModel : ErrorResponseModel
    {
        public IdentityErrorResponseModel(ErrorCode errorCode, string message, List<IdentityError> identityErrors)
            : base(errorCode, message)
        {
            IdentityErrors = identityErrors;
        }

        public List<IdentityError> IdentityErrors { get; }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Common.Exceptions.Response;

namespace Softeq.NetKit.Auth.Common.Utility.Authorization
{
    public class AuthorizationStatus
    {
        public AuthorizationStatus(AuthorizationStatusEnum status, ErrorResponseModel error)
        {
            Status = status;
            Error = error;
        }

        public AuthorizationStatusEnum Status { get; }

        public ErrorResponseModel Error { get; }
    }
}

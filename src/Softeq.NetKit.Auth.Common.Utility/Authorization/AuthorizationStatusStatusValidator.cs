// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Common.Exceptions.Exceptions;

namespace Softeq.NetKit.Auth.Common.Utility.Authorization
{
    public class AuthorizationStatusStatusValidator : IAuthorizationStatusValidator
    {
	    private const string AuthorizationServiceUnavailable = "Authorization service is unavailable.";
	    private const string UnsupportedAuthorizationStatusException = "Unsupported authorization status: {0}";

	    public void Validate(AuthorizationStatus authorizationStatus)
	    {
		    switch (authorizationStatus.Status)
		    {
			    case AuthorizationStatusEnum.Authorized:
				    return;
			    case AuthorizationStatusEnum.Unauthorized:
                    throw new NetKitAuthUnauthorizedException(authorizationStatus.Error.ErrorCode, authorizationStatus.Error.Message);
				case AuthorizationStatusEnum.Forbidden:
					throw new NetKitAuthForbiddenException(authorizationStatus.Error.ErrorCode, authorizationStatus.Error.Message);
				case AuthorizationStatusEnum.ServiceUnavailable:
				    throw new NetKitAuthForbiddenException(ErrorCode.UnknownError, AuthorizationServiceUnavailable);
			    default:
				    throw new NetKitAuthForbiddenException(ErrorCode.UnsupportedAuthorizationStatus, string.Format(UnsupportedAuthorizationStatusException, authorizationStatus.Status));
		    }
	    }
    }
}

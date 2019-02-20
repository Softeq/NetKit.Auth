// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.Common.Exceptions.Exceptions
{
    public class NetKitAuthForbiddenException : NetKitAuthException
    {
        public NetKitAuthForbiddenException(ErrorCode errorCode)
            : base(errorCode)
        {
        }

        public NetKitAuthForbiddenException(ErrorCode errorCode, string message)
            : base(errorCode, message)
        {
        }

        public NetKitAuthForbiddenException(ErrorCode errorCode, string message, Exception innerException)
            : base(errorCode, message, innerException)
        {
        }
    }
}
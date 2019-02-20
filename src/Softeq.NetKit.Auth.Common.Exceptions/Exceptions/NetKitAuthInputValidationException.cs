// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.Common.Exceptions.Exceptions
{
    public class NetKitAuthInputValidationException : NetKitAuthException
    {
        public NetKitAuthInputValidationException(ErrorCode errorCode)
            : base(errorCode)
        {
        }

        public NetKitAuthInputValidationException(ErrorCode errorCode, string message)
            : base(errorCode, message)
        {
        }

        public NetKitAuthInputValidationException(ErrorCode errorCode, string message, Exception innerException)
            : base(errorCode, message, innerException)
        {
        }
    }
}

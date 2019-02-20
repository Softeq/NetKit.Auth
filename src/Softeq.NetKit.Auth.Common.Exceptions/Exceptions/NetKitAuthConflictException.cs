// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.Common.Exceptions.Exceptions
{
    public class NetKitAuthConflictException : NetKitAuthException
    {
        public NetKitAuthConflictException(ErrorCode errorCode)
            : base(errorCode)
        {
        }

        public NetKitAuthConflictException(ErrorCode errorCode, string message)
            : base(errorCode, message)
        {
        }

        public NetKitAuthConflictException(ErrorCode errorCode, string message, Exception innerException)
            : base(errorCode, message, innerException)
        {
        }
    }
}

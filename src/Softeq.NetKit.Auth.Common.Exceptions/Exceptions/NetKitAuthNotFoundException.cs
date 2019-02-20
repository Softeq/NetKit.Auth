// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.Common.Exceptions.Exceptions
{
    public class NetKitAuthNotFoundException : NetKitAuthException
    {
        public NetKitAuthNotFoundException(ErrorCode errorCode)
            : base(errorCode)
        {
        }

        public NetKitAuthNotFoundException(ErrorCode errorCode, string message)
            : base(errorCode, message)
        {
        }

        public NetKitAuthNotFoundException(ErrorCode errorCode, string message, Exception innerException)
            : base(errorCode, message, innerException)
        {
        }
    }
}

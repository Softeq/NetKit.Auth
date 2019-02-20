// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.Common.Exceptions.Exceptions
{
    [Serializable]
    public abstract class NetKitAuthException : Exception
    {
        protected NetKitAuthException(ErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }

        protected NetKitAuthException(ErrorCode errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        protected NetKitAuthException(ErrorCode errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public ErrorCode ErrorCode { get; }
    }
}
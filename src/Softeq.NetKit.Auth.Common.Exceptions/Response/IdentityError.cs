// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Common.Exceptions.Response
{
    public class IdentityError
    {
        public IdentityError(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public string ErrorCode { get; }

        public string Message { get; }
    }
}
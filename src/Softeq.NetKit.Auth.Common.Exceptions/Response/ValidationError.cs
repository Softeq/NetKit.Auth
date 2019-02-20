// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Common.Exceptions.Response
{
    public class ValidationError
    {
        public ValidationError(string field, string errorCode, string message)
        {
            Field = field;
            ErrorCode = errorCode;
            Message = message;
        }

        public string Field { get; }

        public string ErrorCode { get; }

        public string Message { get; }
    }
}
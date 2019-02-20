// Developed by Softeq Development Corporation
// http://www.softeq.com

using Newtonsoft.Json;

namespace Softeq.NetKit.Auth.Common.Exceptions.Response
{
    public class ErrorResponseModel
    {
        public ErrorResponseModel(ErrorCode errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public ErrorCode ErrorCode { get; }

        public string Message { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string StackTrace { get; set; }
    }
}
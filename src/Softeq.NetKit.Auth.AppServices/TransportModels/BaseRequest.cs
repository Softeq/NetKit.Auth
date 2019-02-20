// Developed by Softeq Development Corporation
// http://www.softeq.com

using Newtonsoft.Json;

namespace Softeq.NetKit.Auth.AppServices.TransportModels
{
    public class BaseRequest
    {
        public BaseRequest(string userId)
        {
            UserId = userId;
        }

        [JsonIgnore]
        public string UserId { get; set; }
    }
}

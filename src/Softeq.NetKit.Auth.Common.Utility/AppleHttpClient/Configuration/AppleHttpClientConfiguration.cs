// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Common.Utility.AppleHttpClient.Configuration
{
    public class AppleHttpClientConfiguration
    {
        public const string AppleHttpClientName = "AppleHttpClient";
        public const string RequestUri = "https://appleid.apple.com/auth/token";
        public const string GrantType = "authorization_code";
        public const string Audience = "https://appleid.apple.com";

        public string PrivateKey { get; set; }
        public string RedirectUri { get; set; }
        public string KeyId { get; set; }
        public string TeamId { get; set; }
        public string Lifetime { get; set; }
        public string ClientId { get; set; }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Softeq.NetKit.Auth.Web.Utility
{
    public static class HttpRequestExtensions
    {
        public static bool CheckIsIos(this HttpRequest httpRequest)
        {
            var userAgent = httpRequest.Headers[HeaderNames.UserAgent].ToString().ToLowerInvariant();

            return (userAgent.Contains("iphone") || userAgent.Contains("ipad"));
        }
    }
}
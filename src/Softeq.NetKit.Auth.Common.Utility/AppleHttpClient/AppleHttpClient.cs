// Developed by Softeq Development Corporation
// http://www.softeq.comusing System.Collections.Generic;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EnsureThat;
using Softeq.NetKit.Auth.Common.Utility.AppleHttpClient.Configuration;
using Softeq.NetKit.Auth.Common.Utility.AppleHttpClient.Helpers;
using Softeq.NetKit.Auth.Common.Utility.AppleHttpClient.Interfaces;


namespace Softeq.NetKit.Auth.Common.Utility.AppleHttpClient
{
    public class AppleHttpClient : IAppleHttpClient
    {
        private const string CodeParameter = "code";
        private const string GrantTypeParameter = "grant_type";
        private const string ClientIdParameter = "client_id";
        private const string RedirectUriParameter = "redirect_uri";
        private const string ClientSecretParameter = "client_secret";

        private readonly AppleHttpClientConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AppleHttpClient(IHttpClientFactory httpClientFactory, AppleHttpClientConfiguration configuration)
        {
            _httpClient = Ensure.Any.IsNotNull(httpClientFactory, nameof(httpClientFactory))
                .CreateClient(AppleHttpClientConfiguration.AppleHttpClientName);
            _configuration = Ensure.Any.IsNotNull(configuration, nameof(configuration));
        }

        public async Task<HttpResponseMessage> UserConfirmationAsync(string code)
        {
            var clientSecret = ClientSecretHelper.Create(
                privateKey: _configuration.PrivateKey,
                audience: AppleHttpClientConfiguration.Audience,
                issuer: _configuration.TeamId,
                subject: _configuration.ClientId,
                expires: double.Parse(_configuration.Lifetime),
                keyId: _configuration.KeyId
            );

            KeyValuePair<string, string> Couple(string name, string value) =>
                new KeyValuePair<string, string>(name, value);

            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                Couple(CodeParameter, code),
                Couple(GrantTypeParameter, AppleHttpClientConfiguration.GrantType),
                Couple(ClientIdParameter, _configuration.ClientId),
                Couple(RedirectUriParameter, _configuration.RedirectUri),
                Couple(ClientSecretParameter, clientSecret)
            });

            content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypes.FormUrlEncodedContentType);

            return await _httpClient.PostAsync(AppleHttpClientConfiguration.RequestUri, content);
        }
    }
}

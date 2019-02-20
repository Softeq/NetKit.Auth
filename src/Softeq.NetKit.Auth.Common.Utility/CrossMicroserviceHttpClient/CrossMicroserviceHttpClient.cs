// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.Common.Utility.CrossMicroserviceHttpClient.Exceptions;
using Softeq.NetKit.Auth.Common.Utility.MultiThreading;
using IdentityModel;
using IdentityModel.Client;
using Newtonsoft.Json;
using Polly;

namespace Softeq.NetKit.Auth.Common.Utility.CrossMicroserviceHttpClient
{
    public class CrossMicroserviceHttpClient : ICrossMicroserviceHttpClient
	{
		private readonly AuthToken _accessToken;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ClientConfiguration _clientConfiguration;
		private readonly SemaphoreLocker _getTokenLocker = new SemaphoreLocker();
		private readonly HttpClient _targetMicroserviceHttpClient;
		private readonly HttpClient _identityServerHttpClient;

		public CrossMicroserviceHttpClient(IHttpClientFactory httpClientFactory, ClientConfiguration clientConfiguration, AuthToken authToken)
		{
			_httpClientFactory = httpClientFactory;
			_clientConfiguration = clientConfiguration;
			_accessToken = authToken;
			_targetMicroserviceHttpClient = _httpClientFactory.CreateClient(_clientConfiguration.TargetMicroserviceHttpClientName);
			_identityServerHttpClient = _httpClientFactory.CreateClient(_clientConfiguration.IdentityServerHttpClientName);
		}

		public async Task<HttpResponseMessage> GetAsync(string requestUri)
		{
            return await ExecuteRequest(httpClient => httpClient.GetAsync(requestUri));
		}

		public async Task<HttpResponseMessage> PostAsync(string requestUri, Object content)
		{
			var jsonContent = JsonConvert.SerializeObject(content);

			return await ExecuteRequest(httpClient => httpClient.PostAsync(requestUri,
				new StringContent(jsonContent, Encoding.UTF8, MediaTypes.JsonContentType)));
		}

		public async Task<HttpResponseMessage> PutAsync(string requestUri, Object content)
		{
			var jsonContent = JsonConvert.SerializeObject(content);

			return await ExecuteRequest(httpClient => httpClient.PutAsync(requestUri,
				new StringContent(jsonContent, Encoding.UTF8, MediaTypes.JsonContentType)));
		}

		private async Task<HttpResponseMessage> ExecuteRequest(Func<HttpClient, Task<HttpResponseMessage>> request)
		{
			return await Policy.HandleResult<HttpResponseMessage>(x => x.StatusCode == HttpStatusCode.Unauthorized)
				.RetryAsync(_clientConfiguration.TokenEndpointRetryAttemptsCount, this.GetAuthToken)
				.ExecuteAsync(async () =>
				{
					_targetMicroserviceHttpClient.SetToken(_clientConfiguration.AuthScheme, _accessToken.Value);

					return await request(_targetMicroserviceHttpClient);
				});
		}

		private async Task GetAuthToken(DelegateResult<HttpResponseMessage> result, int retryCount)
		{
			if (_accessToken.Value != null && (int)DateTime.UtcNow.Subtract(_accessToken.Updated).TotalSeconds < _accessToken.ExpiresIn / 2)
			{
				return;
			} 
			
			await _getTokenLocker.LockAsync(async () =>
			{
				if (_accessToken.Value != null && (int)DateTime.UtcNow.Subtract(_accessToken.Updated).TotalSeconds < _accessToken.ExpiresIn / 2)
				{
					return;
				}
				
				await this.ActualizeToken(new ClientCredentialsTokenRequest
				{
					ClientId = _clientConfiguration.ClientId,
					ClientSecret = _clientConfiguration.ClientSecret,
					GrantType = OidcConstants.GrantTypes.ClientCredentials
				});
			});
		}

		private async Task<TokenResponse> ActualizeToken<T>(T request) where T: TokenRequest
		{
			var disco = await _identityServerHttpClient.GetDiscoveryDocumentAsync(_clientConfiguration.IdentityServerUrl);
		    if (disco.IsError)
		    {
		        throw new DiscoveryEndpointException(disco.Error);
			}

			request.Address = disco.TokenEndpoint;

			var response = await _identityServerHttpClient.RequestTokenAsync(request);
			_accessToken.Updated = DateTime.UtcNow;
			_accessToken.Value = response.AccessToken;
			_accessToken.ExpiresIn = response.ExpiresIn;

			return response;
		}
	}
}

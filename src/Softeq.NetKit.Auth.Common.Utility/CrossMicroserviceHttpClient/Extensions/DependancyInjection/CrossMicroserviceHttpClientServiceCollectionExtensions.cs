// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using EnsureThat;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Softeq.NetKit.Auth.Common.Utility.CrossMicroserviceHttpClient.Extensions.DependancyInjection
{
    public static class CrossMicroserviceHttpClientServiceCollectionExtensions
    {
        public static IServiceCollection AddAndRegisterCrossMicroserviceHttpClient(this IServiceCollection services, ClientConfiguration configuration)
        {
            services = AddCrossMicroserviceHttpClientConfiguration(services, configuration);
            AuthToken authToken = new AuthToken();
            services.AddTransient<ICrossMicroserviceHttpClient>(serviceProvider =>
            {
                IHttpClientFactory factory = serviceProvider.GetRequiredService<IHttpClientFactory>();

                return new CrossMicroserviceHttpClient(factory, configuration, authToken);
            });

            return services;
        }

        public static IServiceCollection AddCrossMicroserviceHttpClient(this IServiceCollection services,
                                                                        ClientConfiguration configuration,
                                                                        Func<IServiceCollection, ClientConfiguration, IServiceCollection> confPipelineFunc)
        {
            var serviceCollection = AddCrossMicroserviceHttpClientConfiguration(services, configuration);
            return confPipelineFunc(serviceCollection, configuration);
        }

        private static IServiceCollection AddCrossMicroserviceHttpClientConfiguration(IServiceCollection services, ClientConfiguration configuration)
        {
            Ensure.String.IsNotNullOrWhiteSpace(configuration.TargetMicroserviceUrl, nameof(configuration.TargetMicroserviceUrl));
            Ensure.String.IsNotNullOrWhiteSpace(configuration.IdentityServerUrl, nameof(configuration.IdentityServerUrl));
            Ensure.String.IsNotNullOrWhiteSpace(configuration.ClientId, nameof(configuration.ClientId));
            Ensure.String.IsNotNullOrWhiteSpace(configuration.ClientSecret, nameof(configuration.ClientSecret));
            Ensure.String.IsNotNullOrWhiteSpace(configuration.IdentityServerHttpClientName, nameof(configuration.IdentityServerHttpClientName));
            Ensure.String.IsNotNullOrWhiteSpace(configuration.TargetMicroserviceHttpClientName, nameof(configuration.TargetMicroserviceHttpClientName));

            services.AddHttpClient(configuration.TargetMicroserviceHttpClientName, httpClient => ConfigureTargetMicroserviceHttpClient(httpClient, configuration))
                    .AddTransientHttpErrorPolicy(GetRetryPolicy)
                    .AddPolicyHandler(GetCircuitBreakerPolicy);
            services.AddHttpClient(configuration.IdentityServerHttpClientName)
                    .AddTransientHttpErrorPolicy(GetRetryPolicy)
                    .AddPolicyHandler(GetCircuitBreakerPolicy);

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(PolicyBuilder<HttpResponseMessage> policyBuilder)
        {
            return policyBuilder
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(HttpRequestMessage message)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }

        private static void ConfigureTargetMicroserviceHttpClient(HttpClient httpClient, ClientConfiguration configuration)
        {
            httpClient.BaseAddress = new Uri(configuration.TargetMicroserviceUrl);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypes.JsonContentType));
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            foreach (var header in configuration.CustomHeadersForTargetMicroserviceClient)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }
}

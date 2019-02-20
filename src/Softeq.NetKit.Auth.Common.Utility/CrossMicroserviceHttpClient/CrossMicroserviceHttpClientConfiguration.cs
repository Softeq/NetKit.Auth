// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.NetKit.Auth.Common.Utility.CrossMicroserviceHttpClient
{
	/// <summary>
	/// Used for CrossMicroserviceHttpClient registration in AspNetCore ServiceCollection
	/// </summary>
	public class ClientConfiguration
	{
		private const int MaxGetTokenAttemptsRetryCount = 3;
		private const string DefaultAuthScheme = "Bearer";

		/// <summary>
		/// [Required]
		/// Used for ClientCredentials token request
		/// </summary>
		public string ClientId { get; set; }

		/// <summary>
		/// [Required]
		/// Used for ClientCredentials token request
		/// </summary>
		public string ClientSecret { get; set; }

		/// <summary>
		/// [Required]
		/// This name would be used by IHttpClientFactory for client building and socket management issues
		/// </summary>
		public string IdentityServerHttpClientName { get; set; }

		/// <summary>
		/// [Required]
		/// Used for ClientCredentials token request
		/// In the format of 'https://your-server-domain.example'
		/// </summary>
		public string IdentityServerUrl { get; set; }
		
		/// <summary>
		/// [Required]
		/// In the format of 'https://your-server-domain.example'
		/// </summary>
		public string TargetMicroserviceUrl { get; set; }

		/// <summary>
		/// [Required]
		/// This name would be used by IHttpClientFactory for client building and socket management issues
		/// </summary>
		public string TargetMicroserviceHttpClientName { get; set; }

		/// <summary>
		/// Defaults to 3
		/// </summary>
		public int TokenEndpointRetryAttemptsCount { get; set; } = ClientConfiguration.MaxGetTokenAttemptsRetryCount;

		/// <summary>
		/// Ex. 'Bearer', 'Basic'... Defaults to 'Bearer'
		/// </summary>
		public string AuthScheme { get; set; } = ClientConfiguration.DefaultAuthScheme;

		public IDictionary<string, string> CustomHeadersForTargetMicroserviceClient { get; set; } = new Dictionary<string, string>();
	}
}

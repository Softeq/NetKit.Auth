// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.Common.Utility.CrossMicroserviceHttpClient.Exceptions
{
	public class DiscoveryEndpointException: Exception
	{
		private const string DiscoveryEndpoint = "DiscoveryEndpoint Error:";

		public DiscoveryEndpointException(string errorMessage) : base($"{DiscoveryEndpoint} {errorMessage}")
		{
		}
	}
}

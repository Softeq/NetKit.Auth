// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.Common.Utility.CrossMicroserviceHttpClient
{
	public class AuthToken
	{
		public string Value { get; set; }
		public int ExpiresIn { get; set; }
		public DateTime Updated { get; set; }
	}
}

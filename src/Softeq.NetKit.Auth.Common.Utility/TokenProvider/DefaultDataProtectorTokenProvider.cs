// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using Softeq.NetKit.Auth.Common.Utility.Extensions;
using Softeq.NetKit.Auth.Common.Utility.TokenProvider.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Softeq.NetKit.Auth.Common.Utility.TokenProvider
{
	public class DefaultDataProtectorTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
	{
		private const string ActivationCodeExpired = "Activate code has expired.";

		public IDataProtector DataProtector { get; set; }

		public DefaultDataProtectorTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DefaultDataProtectorTokenProviderOptions> options)
			: base(dataProtectionProvider, options)
		{
			DataProtector = Protector;
		}

		public string Validate(string token)
		{
			var unprotectedData = Protector.Unprotect(Convert.FromBase64String(token));
			using (var ms = new MemoryStream(unprotectedData))
			{
				using (var reader = ms.CreateBinaryFileReader())
				{
					var creationTime = reader.ReadTokenCreationDate();
					var expirationTime = creationTime + Options.TokenLifespan;
					if (expirationTime < DateTimeOffset.UtcNow)
					{
						throw new InvalidOperationException(ActivationCodeExpired);
					}
					var userId = reader.ReadString();
					return userId;
				}
			}
		}

		public TokenResponse GetTokenInformation(string token)
		{
			var unprotectedData = Protector.Unprotect(Convert.FromBase64String(token));
			using (var ms = new MemoryStream(unprotectedData))
			{
				using (var reader = ms.CreateBinaryFileReader())
				{
					reader.ReadTokenCreationDate();
					var userId = reader.ReadString();
					var purpose = reader.ReadString();
					Enum.TryParse(purpose, out TokenPurposeEnum tokenPurpose);
					return new TokenResponse
					{
						Purpose = tokenPurpose,
						UserId = userId
					};
				}
			}
		}
	}

	public class DefaultDataProtectorTokenProviderOptions : DataProtectionTokenProviderOptions { }
}

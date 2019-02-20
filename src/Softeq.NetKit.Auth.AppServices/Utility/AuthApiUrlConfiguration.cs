// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.AppServices.Utility
{
	public class AuthApiUrlConfiguration
	{
		private readonly string _resetPasswordPath;
		private readonly string _confirmEmailPath;

		public AuthApiUrlConfiguration(string apiUrl, string resetPasswordPath, string confirmEmailPath)
		{
			ApiUrl = apiUrl;
			_resetPasswordPath = resetPasswordPath;
			_confirmEmailPath = confirmEmailPath;
		}

		public string ApiUrl { get; private set; }

		public string ResetPasswordUrl
		{
			get
			{
				var builder = new UriBuilder(ApiUrl)
				{
					Path = _resetPasswordPath
				};

				return builder.ToString();
			}
		}

		public string ConfirmEmailUrl
		{
			get
			{
				var builder = new UriBuilder(ApiUrl)
				{
					Path = _confirmEmailPath
				};

				return builder.ToString();
			}
		}
	}
}
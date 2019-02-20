// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Web;

namespace Softeq.NetKit.Auth.AppServices.Utility
{
	public static class UrlHelperExtensions
	{
		public static string ApiCallbackLink(string url, string code)
		{
			var uriBilder = new UriBuilder(url);
			var query = HttpUtility.ParseQueryString(uriBilder.Query);
			query["code"] = code;
			uriBilder.Query = query.ToString();
			return uriBilder.ToString();
		}
	}
}
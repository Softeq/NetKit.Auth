// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Softeq.NetKit.Auth.Common.Utility.CrossMicroserviceHttpClient
{
	public interface ICrossMicroserviceHttpClient
	{
		Task<HttpResponseMessage> GetAsync(string requestUri);
		Task<HttpResponseMessage> PostAsync(string requestUri, Object content);
		Task<HttpResponseMessage> PutAsync(string requestUri, Object content);
	}
}

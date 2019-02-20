// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Softeq.NetKit.Auth.Web.Middleware
{
    public class ApiKeyAuthorizationRequirement : IAuthorizationRequirement
	{
    }

	public class ApiKeyAuthorizationHandler : AuthorizationHandler<ApiKeyAuthorizationRequirement>
	{
		private readonly IConfiguration _configuration;

		public ApiKeyAuthorizationHandler(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyAuthorizationRequirement requirement)
		{
			var httpContext = context.Resource as AuthorizationFilterContext;
			if (!httpContext.HttpContext.Request.Headers.Keys.Contains(_configuration[ConfigurationSettings.AuthorizationHeaderName]))
			{
				context.Fail();
				httpContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}
			else if (httpContext.HttpContext.Request.Headers[_configuration[ConfigurationSettings.AuthorizationHeaderName]] !=
			         _configuration[ConfigurationSettings.AuthorizationKey])
			{
				context.Fail();
				httpContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			}
			else
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}

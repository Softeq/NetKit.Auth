// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Security.Claims;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.AppServices.Abstract;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;

namespace Softeq.NetKit.Auth.Web.Middleware
{
	public class PendingAllowedAuthorizationRequirement : IAuthorizationRequirement
	{
	}

	public class PendingAllowedAuthorizationRequirementHandler : AuthorizationHandler<PendingAllowedAuthorizationRequirement>
	{
		private readonly IUserService _userService;

		public PendingAllowedAuthorizationRequirementHandler(IUserService userService)
		{
			_userService = userService;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
						PendingAllowedAuthorizationRequirement requirement)
		{
			if (context.User.Identity.IsAuthenticated)
			{
				var saasUserId = context.User.FindFirstValue(JwtClaimTypes.Subject);
				await _userService.CheckPendingAllowedAuthRequirementAsync(saasUserId);
				context.Succeed(requirement);
			}
			else
			{
			    context.Fail();
			}
		}
	}
}

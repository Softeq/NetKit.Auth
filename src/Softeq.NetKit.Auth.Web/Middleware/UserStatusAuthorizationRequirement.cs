// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.Auth.AppServices.Abstract;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Softeq.NetKit.Auth.Web.Middleware
{
	public class UserStatusAuthorizationRequirement : IAuthorizationRequirement
	{
	}

	public class UserStatusAuthorizationHandler : AuthorizationHandler<UserStatusAuthorizationRequirement>
	{
		private readonly IUserService _userService;

		public UserStatusAuthorizationHandler(IUserService userService)
		{
			_userService = userService;
		}

	    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserStatusAuthorizationRequirement requirement)
	    {
		    if (context.User.Identity.IsAuthenticated)
		    {
			    var saasUserId = context.User.GetSubjectId();
				await _userService.ValidateStatusAsync(saasUserId);
			    context.Succeed(requirement);
			}
		    else
		    {
		        context.Fail();
			}
		}
	}
}

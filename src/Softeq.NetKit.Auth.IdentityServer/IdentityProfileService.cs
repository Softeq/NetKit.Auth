// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Security.Claims;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.AppServices.Abstract;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Common.Exceptions.Exceptions;
using Softeq.NetKit.Auth.Domain.Models;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Interfaces;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace Softeq.NetKit.Auth.IdentityServer
{
    public class IdentityProfileService : IProfileService
    {
        private const string EmailWasNotConfirmed = "Email is not confirmed.";
        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;
        private readonly IUserDomainService _userDomainService;
        private readonly IUserService _userService;

        public IdentityProfileService(IUserClaimsPrincipalFactory<User> claimsFactory,
            IUserDomainService userDomainService,
            IUserService userService)
        {
            _claimsFactory = claimsFactory;
            _userDomainService = userDomainService;
            _userService = userService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userDomainService.GetUserByIdAsync(sub);
			if (!user.EmailConfirmed)
            {
                throw new NetKitAuthForbiddenException(ErrorCode.EmailIsNotConfirmed, EmailWasNotConfirmed);
            }

            var principal = await _claimsFactory.CreateAsync(user);
            if (!principal.IsInRole(Roles.User))
            {
                context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            }

            context.AddRequestedClaims(principal.Claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
			try
			{
				await _userService.ValidateStatusAsync(sub);
			}
			catch (NetKitAuthForbiddenException exception)
			{
				if (exception.ErrorCode != ErrorCode.UserIsInPendingState)
				{
					throw;
				}
			}

	        await _userService.CheckIfPasswordHasExpired(sub);
		}
	}
}
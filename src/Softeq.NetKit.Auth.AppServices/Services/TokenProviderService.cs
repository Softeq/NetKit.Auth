// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.Auth.AppServices.Abstract;
using Softeq.NetKit.Auth.Domain.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Softeq.NetKit.Auth.AppServices.Services
{
	public class TokenProviderService : ITokenProviderService
	{
		private readonly UserManager<User> _userManager;

		public TokenProviderService(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

	    public Task<string> GenerateEmailConfirmationTokenAsync(User user)
	    {
	        return _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

	    public Task<string> GeneratePasswordResetTokenAsync(User user)
		{
			return _userManager.GeneratePasswordResetTokenAsync(user);
		}
	}
}
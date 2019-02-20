// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.Auth.Domain.Models.User;

namespace Softeq.NetKit.Auth.AppServices.Abstract
{
	public interface ITokenProviderService
	{
		Task<string> GenerateEmailConfirmationTokenAsync(User user);

	    Task<string> GeneratePasswordResetTokenAsync(User user);
    }
}
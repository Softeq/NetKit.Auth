// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Models;

namespace Softeq.NetKit.Auth.DomainServices.Interfaces.Interfaces
{
    public interface IUserDomainService
    {
        Task<User> GetUserByIdAsync(string userId);
	    Task<string> GetUserRoleNameAsync(string userId);
	    Task<UserDomainModel> AcceptTermsOfUseAndPrivacyPolicyAsync(string userId);
	    Task ActivateUserAsync(string userId);
	    Task ConfirmEmailAsync(string userId);
		Task UpdateUserAsync(User user);
		Task<UserStatusDomainModel> GetUserStatusByUserIdAsync(string userId);
		Task DeleteUserAsync(string saasUserId);
	    Task DeactivateUserAsync(string userId);
	    Task CheckIfPasswordHasExpired(string userId, int passwordActivePeriodInDays);
	    Task UpdateLastPasswordChangeDateAsync(string userId);
	    Task UpdateLastPasswordExpiresEmailSentDateAsync(string userId);
	    Task UpdateLastPasswordExpiredEmailSentDateAsync(string userId);
	    Task UpdateLastAccountFailedAttemptsEmailSentDateAsync(string userId);
		Task<User> GetUserByNameAsync(string userName);
        Task<bool> IsFirstRoleRepresentativeAsync(string userId, string role);
        Task<User> GetUserByAppleKeyAsync(string appleKey);
    }
}

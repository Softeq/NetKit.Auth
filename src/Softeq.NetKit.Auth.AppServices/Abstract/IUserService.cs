// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.Auth.AppServices.TransportModels;
using Softeq.NetKit.Auth.AppServices.TransportModels.User.Request;
using Softeq.NetKit.Auth.AppServices.TransportModels.User.Response;
using Softeq.NetKit.Auth.Common.Utility.Authorization;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Models;

namespace Softeq.NetKit.Auth.AppServices.Abstract
{
    public interface IUserService
    {
        Task AcceptTermsOfUseAndPrivacyPolicyAsync(BaseRequest request);
        Task<UserResponse> GetUserInfoAsync(BaseRequest request);
	    Task ActivateUserAsync(BaseRequest request);
	    Task ConfirmEmailAsync(BaseRequest request);
		Task<UserStatusDomainModel> GetUserStatusByUserIdAsync(string userId);
		Task CheckPendingAllowedAuthRequirementAsync(string saasUserId);
		Task DeleteUserAsync(string saasUserId);
	    Task<AuthorizationStatus> GetAuthorizationStatusAsync(string saasUserId);
        Task UpdateAsync(UpdateUserDomainModel model);
	    Task DeactivateUserAsync(BaseRequest request);
	    Task ValidateStatusAsync(string saasUserId);
        Task SendEmailConfirmationEmailAsync(User user);
        Task SendResetPasswordEmailAsync(User user);
        Task SendResetPasswordEmailAsync(string userId, EmailPurposeEnum emailPurpose);
        Task SendChangePasswordEmailAsync(User user);
        Task CheckIfPasswordHasExpired(string userId);
	    Task UpdateLastAccountFailedAttemptsEmailSentDateAsync(string userId);
	    Task UpdateLastPasswordChangedDateAsync(string userId);
	    Task UpdateLastPasswordExpiredEmailSentDateAsync(string userId);
	    Task UpdateLastPasswordExpiresEmailSentDateAsync(string userId);
	    Task<bool> ValidateCredentialsAsync(string userName, string password);
        Task<bool> IsFirstRoleRepresentativeAsync(string userId, string role);
    }
}

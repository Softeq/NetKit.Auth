// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using AutoMapper;
using Softeq.NetKit.Auth.AppServices.Abstract;
using Softeq.NetKit.Auth.AppServices.EmailNotifications;
using Softeq.NetKit.Auth.AppServices.TransportModels;
using Softeq.NetKit.Auth.AppServices.TransportModels.User.Request;
using Softeq.NetKit.Auth.AppServices.TransportModels.User.Response;
using Softeq.NetKit.Auth.AppServices.Utility;
using Softeq.NetKit.Auth.Common.EmailTemplates;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Common.Exceptions.Exceptions;
using Softeq.NetKit.Auth.Common.Exceptions.Response;
using Softeq.NetKit.Auth.Common.Utility.Authorization;
using Softeq.NetKit.Auth.Domain.Models;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Interfaces;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Models;
using Softeq.NetKit.Auth.Integration.Email;
using Softeq.NetKit.Auth.Integration.Email.Dto;
using Serilog;
using UrlHelperExtensions = Softeq.NetKit.Auth.AppServices.Utility.UrlHelperExtensions;

namespace Softeq.NetKit.Auth.AppServices.Services
{
	public class UserService : BaseService, IUserService
	{
		private const string UserIsDeleted = "User is in deleted state";
		private const string InactiveUserMessage = "The user identity has inactive status.";

		private const string PendingAllowedRequirementNotMet =
			"In order to get the resource User has to be in Pending or Active state";

		private const string UserIsPendingMessage = "User is in pending state.";
		private const string UnsupportedRoleException = "Unsupported role: {0}";
		private const string UnsupportedEmailPurposeException = "Unsupported email purpose: {0}";

		private readonly IUserDomainService _userDomainService;
		private readonly IAuthorizationStatusValidator _authorizationStatusValidator;
		private readonly ITokenProviderService _tokenProviderService;
		private readonly IPasswordHistoryService _passwordHistoryService;
		private readonly AuthApiUrlConfiguration _authApiUrlConfiguration;
		private readonly PasswordConfiguration _passwordConfiguration;
	    private readonly IEmailService _emailService;
	    private readonly IEmailTemplateProvider _emailTemplateProvider;
	    private readonly EmailTemplatesConfiguration _emailTemplatesConfiguration;

        public UserService(ILogger logger, IMapper mapper,
			IServiceProvider serviceProvider,
			IUserDomainService userDomainService,
			IAuthorizationStatusValidator authorizationStatusValidator,
			ITokenProviderService tokenProviderService,
			AuthApiUrlConfiguration authApiUrlConfiguration,
			PasswordConfiguration passwordConfiguration,
			IPasswordHistoryService passwordHistoryService,
            IEmailService emailService,
            IEmailTemplateProvider emailTemplateProvider,
            EmailTemplatesConfiguration emailTemplatesConfiguration)
			: base(logger, mapper, serviceProvider)
		{
			_userDomainService = userDomainService;
			_authorizationStatusValidator = authorizationStatusValidator;
			_tokenProviderService = tokenProviderService;
			_authApiUrlConfiguration = authApiUrlConfiguration;
			_passwordConfiguration = passwordConfiguration;
			_passwordHistoryService = passwordHistoryService;
		    _emailService = emailService;
		    _emailTemplateProvider = emailTemplateProvider;
		    _emailTemplatesConfiguration = emailTemplatesConfiguration;
		}

		public Task AcceptTermsOfUseAndPrivacyPolicyAsync(BaseRequest request)
		{
            return _userDomainService.AcceptTermsOfUseAndPrivacyPolicyAsync(request.UserId);
        }

		public async Task<UserResponse> GetUserInfoAsync(BaseRequest request)
		{
			var user = await _userDomainService.GetUserByIdAsync(request.UserId);
			return Mapper.Map<User, UserResponse>(user);
		}

		public Task ActivateUserAsync(BaseRequest request)
		{
            return _userDomainService.ActivateUserAsync(request.UserId);
        }

		public Task ConfirmEmailAsync(BaseRequest request)
		{
            return _userDomainService.ConfirmEmailAsync(request.UserId);
        }

		public async Task ValidateStatusAsync(string saasUserId)
		{
			var authorizationStatus = await GetAuthorizationStatusAsync(saasUserId);
			_authorizationStatusValidator.Validate(authorizationStatus);
	    }

	    public async Task SendEmailConfirmationEmailAsync(User user)
	    {
	        var token = await _tokenProviderService.GenerateEmailConfirmationTokenAsync(user);
	        var callbackLink = UrlHelperExtensions.ApiCallbackLink(_authApiUrlConfiguration.ConfirmEmailUrl, token);

	        var baseEmailTemplate = _emailTemplateProvider.GetBaseTemplateHtml();

            var emailConfirmationEmailTemplate = _emailTemplateProvider.GetTemplateHtml(
	            _emailTemplatesConfiguration.EmailConfirmationEmailTemplateName);

	        await _emailService.SendNotificationAsync(new UserConfirmationEmailNotification(
	            new UserConfirmationEmailModel(callbackLink, user.Email),
	            emailConfirmationEmailTemplate,
	            baseEmailTemplate,
	            new RecipientDto(user.Email, user.UserName)));
	    }

        public Task SendResetPasswordEmailAsync(User user)
	    {
            return SendResetPasswordEmailAsync(user, EmailPurposeEnum.ResetPassword);
        }

	    public async Task SendResetPasswordEmailAsync(string userId, EmailPurposeEnum emailPurpose)
		{
			var user = await _userDomainService.GetUserByIdAsync(userId);

		    await SendResetPasswordEmailAsync(user, emailPurpose);
	    }

        public async Task<User> GetUserByAppleKeyAsync(string appleKey)
        {
            return await _userDomainService.GetUserByAppleKeyAsync(appleKey);
        }

	    public async Task SendChangePasswordEmailAsync(User user)
	    {
	        var resetPasswordToken = await _tokenProviderService.GeneratePasswordResetTokenAsync(user);
	        var callbackUrl = UrlHelperExtensions.ApiCallbackLink(_authApiUrlConfiguration.ResetPasswordUrl, resetPasswordToken);

	        var baseEmailTemplate = _emailTemplateProvider.GetBaseTemplateHtml();

            var changePasswordEmailTemplate = _emailTemplateProvider.GetTemplateHtml(
	            _emailTemplatesConfiguration.ChangePasswordEmailTemplateName);

	        await _emailService.SendNotificationAsync(new ChangePasswordEmailNotification(
	            new ChangePasswordEmailModel(callbackUrl, user.Email),
	            new RecipientDto(user.Email, user.UserName),
	            baseEmailTemplate,
	            changePasswordEmailTemplate));
	    }

	    public async Task CheckIfPasswordHasExpired(string userId)
		{
			try
			{
				await _userDomainService.CheckIfPasswordHasExpired(userId, _passwordConfiguration.ActivePeriodInDays);
			}
			catch (NetKitAuthConflictException ex)
			{
				await SendResetPasswordEmailAsync(userId, EmailPurposeEnum.PasswordExpiration);
				throw new NetKitAuthUnauthorizedException(ex.ErrorCode, ex.Message, ex.InnerException);
			}
		}

		public Task UpdateLastAccountFailedAttemptsEmailSentDateAsync(string userId)
		{
            return _userDomainService.UpdateLastAccountFailedAttemptsEmailSentDateAsync(userId);
        }

		public Task UpdateLastPasswordChangedDateAsync(string userId)
		{
            return _userDomainService.UpdateLastPasswordChangeDateAsync(userId);
        }

		public Task UpdateLastPasswordExpiredEmailSentDateAsync(string userId)
		{
            return _userDomainService.UpdateLastPasswordExpiredEmailSentDateAsync(userId);
        }

		public Task UpdateLastPasswordExpiresEmailSentDateAsync(string userId)
		{
            return _userDomainService.UpdateLastPasswordExpiresEmailSentDateAsync(userId);
        }

		public async Task<bool> ValidateCredentialsAsync(string userName, string password)
		{
			var user = await _userDomainService.GetUserByNameAsync(userName);
			return _passwordHistoryService.VerifyHashedPassword(user, user.PasswordHash, password);
		}

		public Task<bool> IsFirstRoleRepresentativeAsync(string userId, string role)
		{
			return _userDomainService.IsFirstRoleRepresentativeAsync(userId, role);
		}

		public Task<UserStatusDomainModel> GetUserStatusByUserIdAsync(string userId)
		{
			return _userDomainService.GetUserStatusByUserIdAsync(userId);
		}

		public async Task<AuthorizationStatus> GetAuthorizationStatusAsync(string saasUserId)
		{
			UserStatusDomainModel userStatus;

			var roleName = await _userDomainService.GetUserRoleNameAsync(saasUserId);
			switch (roleName)
			{
				case Roles.User:
				case Roles.Admin:
					userStatus = await _userDomainService.GetUserStatusByUserIdAsync(saasUserId);
					break;
				default:
					return new AuthorizationStatus(AuthorizationStatusEnum.Forbidden, new ErrorResponseModel(ErrorCode.UnsupportedRole, string.Format(UnsupportedRoleException, roleName)));
			}

            if (userStatus != null)
            {
                switch ((UserStatusEnum)userStatus.Id)
                {
                    case UserStatusEnum.Pending:
                        return new AuthorizationStatus(AuthorizationStatusEnum.Forbidden, new ErrorResponseModel(ErrorCode.UserIsInPendingState, UserIsPendingMessage));
                    case UserStatusEnum.Deleted:
                        return new AuthorizationStatus(AuthorizationStatusEnum.Unauthorized, new ErrorResponseModel(ErrorCode.UserIsInDeletedState, UserIsDeleted));
                    case UserStatusEnum.Inactive:
                        return new AuthorizationStatus(AuthorizationStatusEnum.Unauthorized, new ErrorResponseModel(ErrorCode.InactiveUserState, InactiveUserMessage));
                }
            }

            return new AuthorizationStatus(AuthorizationStatusEnum.Authorized, null);
		}

		public async Task CheckPendingAllowedAuthRequirementAsync(string saasUserId)
		{
			var userStatus = await _userDomainService.GetUserStatusByUserIdAsync(saasUserId);

			if (userStatus.Id != (int) UserStatusEnum.Pending && userStatus.Id != (int) UserStatusEnum.Active)
			{
				throw new NetKitAuthUnauthorizedException(ErrorCode.PendingAllowedRequirementFailed,
					PendingAllowedRequirementNotMet);
			}
		}

		public Task DeleteUserAsync(string saasUserId)
		{
            return _userDomainService.DeleteUserAsync(saasUserId);
        }

		public async Task UpdateAsync(UpdateUserDomainModel model)
		{
			var user = await _userDomainService.GetUserByIdAsync(model.UserId);
			var updatingUser = Mapper.Map(model, user);

			await _userDomainService.UpdateUserAsync(updatingUser);
		}

		public Task DeactivateUserAsync(BaseRequest request)
		{
            return _userDomainService.DeactivateUserAsync(request.UserId);
        }
        
        private async Task SendResetPasswordEmailAsync(User user, EmailPurposeEnum emailPurpose)
        {
            var resetPasswordToken = await _tokenProviderService.GeneratePasswordResetTokenAsync(user);
            var callbackUrl =
                UrlHelperExtensions.ApiCallbackLink(_authApiUrlConfiguration.ResetPasswordUrl, resetPasswordToken);

            string baseEmailTemplate;
            switch (emailPurpose)
            {
                case EmailPurposeEnum.ResetPassword:
                    baseEmailTemplate = _emailTemplateProvider.GetBaseTemplateHtml();

                    var resetPasswordEmailTemplate = _emailTemplateProvider.GetTemplateHtml(
                        _emailTemplatesConfiguration.ResetPasswordEmailTemplateName);

                    await _emailService.SendNotificationAsync(new ResetPasswordEmailNotification(
                        new ResetPasswordEmailModel(callbackUrl, user.Email),
                        new RecipientDto(user.Email, user.UserName),
                        baseEmailTemplate,
                        resetPasswordEmailTemplate));

                    break;

                case EmailPurposeEnum.PasswordExpiration:
                    baseEmailTemplate = _emailTemplateProvider.GetBaseTemplateHtml();

                    var passwordHasExpiredEmailTemplate = _emailTemplateProvider.GetTemplateHtml(
                        _emailTemplatesConfiguration.PasswordHasExpiredEmailTemplateName);

                    if (!user.LastPasswordExpiredEmailSentDate.HasValue ||
                        user.LastPasswordExpiredEmailSentDate.Value.AddMinutes(_passwordConfiguration
                            .TokenLifeTimeInMinutes) <= DateTimeOffset.UtcNow)
                    {
                        await _emailService.SendNotificationAsync(new PasswordHasExpiredEmailNotification(
                            new PasswordHasExpiredEmailModel(callbackUrl, user.Email),
                            new RecipientDto(user.Email, user.UserName),
                            baseEmailTemplate,
                            passwordHasExpiredEmailTemplate));

                        await UpdateLastPasswordExpiredEmailSentDateAsync(user.Id);
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(emailPurpose), emailPurpose,
                        UnsupportedEmailPurposeException);
            }
        }
    }
}
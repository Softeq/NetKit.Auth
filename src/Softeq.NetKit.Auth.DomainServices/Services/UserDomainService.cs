// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using AutoMapper;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Common.Exceptions.Exceptions;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Interfaces;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Models;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;
using Softeq.NetKit.Auth.Repository.Interfaces;

namespace Softeq.NetKit.Auth.DomainServices.Services
{
    public class UserDomainService : BaseDomainService<IAuthUnitOfWork>, IUserDomainService
    {
        private const string UserNotFoundException = "User does not exist.";
        private const string TermsOfUseAreAcceptedException = "Terms of service had already been accepted previousely.";
        private const string UserAlreadyInDeletedState = "User is already in deleted state";
        private const string UserIsNotActivatedMessage = "User is not in Active state.";
        private const string UserHasNoRole = "User has no role assigned";
	    private const string UserIsNotPendingOrInactivatedMessage = "User is not in Pending or Inactive state.";
	    private const string PasswordHasExpiredMessage = "User password has expired.";

        public UserDomainService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            User user;
            try
            {
                user = await UnitOfWork.ReadUserRepository.GetUserByIdAsync(userId);
            }
            catch (InvalidOperationException exception)
            {
                throw new NetKitAuthNotFoundException(ErrorCode.UserNotFound, UserNotFoundException, exception);
            }

            return user;
        }

        public async Task<string> GetUserRoleNameAsync(string userId)
        {
            try
            {
                return (await UnitOfWork.ReadUserRolesRepository.GetUserRoleByUserIdAsync(userId)).Role.Name;
            }
            catch (InvalidOperationException exception)
            {
                throw new NetKitAuthNotFoundException(ErrorCode.UserRoleNotFound, UserHasNoRole, exception);
            }
        }

        public async Task<UserDomainModel> AcceptTermsOfUseAndPrivacyPolicyAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user.IsAcceptedTermsAndPolicy)
            {
                throw new NetKitAuthConflictException(ErrorCode.UserHadAlreadyAcceptedTermsAndPolicies, TermsOfUseAreAcceptedException);
            }

            user.IsAcceptedTermsAndPolicy = true;
            await UpdateUserAsync(user);
            return Mapper.Map<User, UserDomainModel>(user);
        }

        public async Task ActivateUserAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);

            if (user.StatusId != (int)UserStatusEnum.Pending && user.StatusId != (int)UserStatusEnum.Inactive)
            {
                throw new NetKitAuthConflictException(ErrorCode.UserIsNotInPendingOrInactiveState, UserIsNotPendingOrInactivatedMessage);
            }

            user.StatusId = (int)UserStatusEnum.Active;
            await UpdateUserAsync(user);
        }

        public async Task ConfirmEmailAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);
            user.EmailConfirmed = true;
            user.StatusId = (int)UserStatusEnum.Active;
            await UpdateUserAsync(user);
        }

        public Task UpdateUserAsync(User user)
        {
            UnitOfWork.WriteUserRepository.Update(user);
            return UnitOfWork.SaveChangesAsync();
        }

        public async Task<UserStatusDomainModel> GetUserStatusByUserIdAsync(string userId)
        {
            try
            {
                var userStatus = await UnitOfWork.ReadUserRepository.GetUserStatusByUserIdAsync(userId);
                return Mapper.Map<UserStatusDomainModel>(userStatus);
            }
            catch (InvalidOperationException exception)
            {
                throw new NetKitAuthNotFoundException(ErrorCode.UserNotFound, UserNotFoundException, exception);
            }
        }

        public async Task DeleteUserAsync(string saasUserId)
        {
            var user = await GetUserByIdAsync(saasUserId);

            if (user.StatusId == (int)UserStatusEnum.Deleted)
            {
                throw new NetKitAuthConflictException(ErrorCode.UserIsAlreadyInDeletedState, UserAlreadyInDeletedState);
            }

            user.StatusId = (int)UserStatusEnum.Deleted;

            user.DeletedUserInfo = new DeletedUserInfo
            {
                UserId = user.Id,
                Email = user.Email,
                NormalizedEmail = user.NormalizedEmail,
                UserName = user.UserName,
                NormalizedUserName = user.NormalizedUserName
            };

            user.Email = null;
            user.NormalizedEmail = null;
            user.UserName = null;
            user.NormalizedUserName = null;

            UnitOfWork.WriteUserRepository.Update(user);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task DeactivateUserAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);

            if (user.StatusId != (int)UserStatusEnum.Active)
            {
                throw new NetKitAuthConflictException(ErrorCode.UserIsNotInActiveState, UserIsNotActivatedMessage);
            }

            user.StatusId = (int)UserStatusEnum.Inactive;
            await UpdateUserAsync(user);
        }

	    public async Task CheckIfPasswordHasExpired(string userId, int passwordActivePeriodInDays)
	    {
			var user = await GetUserByIdAsync(userId);

		    if (!user.LastPasswordChangedDate.HasValue && user.Created.AddDays(passwordActivePeriodInDays) < DateTimeOffset.UtcNow || 
				user.LastPasswordChangedDate.HasValue && user.LastPasswordChangedDate.Value.AddDays(passwordActivePeriodInDays) < DateTimeOffset.UtcNow)
		    {
				throw new NetKitAuthConflictException(ErrorCode.PasswordHasExpired, PasswordHasExpiredMessage);
			}
		}

	    public async Task UpdateLastPasswordChangeDateAsync(string userId)
	    {
		    var user = await GetUserByIdAsync(userId);
		    user.LastPasswordChangedDate = DateTimeOffset.UtcNow;
		    await UpdateUserAsync(user);
		}

	    public async Task UpdateLastPasswordExpiresEmailSentDateAsync(string userId)
	    {
			var user = await GetUserByIdAsync(userId);
		    user.LastPasswordExpiresEmailSentDate = DateTimeOffset.UtcNow;
		    await UpdateUserAsync(user);
		}

	    public async Task UpdateLastPasswordExpiredEmailSentDateAsync(string userId)
	    {
			var user = await GetUserByIdAsync(userId);
		    user.LastPasswordExpiredEmailSentDate = DateTimeOffset.UtcNow;
		    await UpdateUserAsync(user);
		}

	    public async Task UpdateLastAccountFailedAttemptsEmailSentDateAsync(string userId)
	    {
			var user = await GetUserByIdAsync(userId);
		    user.LastAccountFailedAttemptsEmailSentDate = DateTimeOffset.UtcNow;
		    await UpdateUserAsync(user);
		}

	    public async Task<User> GetUserByNameAsync(string userName)
	    {
		    try
		    {
			    return await UnitOfWork.ReadUserRepository.GetUserByNameAsync(userName);
		    }
		    catch (InvalidOperationException exception)
		    {
			    throw new NetKitAuthNotFoundException(ErrorCode.UserNotFound, UserNotFoundException, exception);
		    }
		}

        public async Task<bool> IsFirstRoleRepresentativeAsync(string userId, string role)
        {
            var user = await UnitOfWork.ReadUserRepository.GetFirstRoleRepresentativeAsync(role);
            if (user == null)
            {
                throw new NetKitAuthNotFoundException(ErrorCode.UserNotFound, UserNotFoundException);
            }

            return user.Id == userId;
        }
    }
}

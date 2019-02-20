// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Softeq.NetKit.Auth.AppServices.Abstract;
using Softeq.NetKit.Auth.AppServices.Utility;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Common.Exceptions.Exceptions;
using Softeq.NetKit.Auth.Common.Utility.Hashing;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Interfaces;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Softeq.NetKit.Auth.AppServices.Services
{
	public class PasswordHistoryService : BaseService, IPasswordHistoryService
	{
		private const string PasswordHashIsNotUniqueMessage = "New password is not unique from the previous ones";

		private readonly IPasswordHistoryDomainService _passwordHistoryDomainService;
		private readonly IUserDomainService _userDomainService;
		private readonly DefaultPasswordHasher<User> _passwordHasher;
		private readonly PasswordConfiguration _passwordConfiguration;

		public  PasswordHistoryService(ILogger logger, 
			IMapper mapper, 
			IServiceProvider serviceProvider, 
			IPasswordHistoryDomainService passwordHistoryDomainService, 
			DefaultPasswordHasher<User> passwordHasher, 
			IUserDomainService userDomainService, 
			PasswordConfiguration passwordConfiguration) : base(logger, mapper, serviceProvider)
		{
			_passwordHistoryDomainService = passwordHistoryDomainService;
			_passwordHasher = passwordHasher;
			_userDomainService = userDomainService;
			_passwordConfiguration = passwordConfiguration;
		}

		public async Task AddPasswordHistoryAsync(UserPasswordDomainModel model)
		{
			var addPasswordHistoryDomainModel = Mapper.Map<AddPasswordHistoryDomainModel>(model);
			addPasswordHistoryDomainModel.PasswordHash = await HashPasswordAsync(model.UserId, model.Password);
			await _passwordHistoryDomainService.AddPasswordHistoryAsync(addPasswordHistoryDomainModel);
		}

		public async Task CheckAndAddPasswordHistoryAsync(UserPasswordDomainModel model)
		{
			var user = await _userDomainService.GetUserByIdAsync(model.UserId);
			var getLastPasswordHashesDomainModel = Mapper.Map<GetLastPasswordHashesDomainModel>(model);
			var removeUnusedPasswordHashesDomainModel = Mapper.Map<RemoveUnusedPasswordHashesDomainModel>(model);
			getLastPasswordHashesDomainModel.PasswordUniqueCount = removeUnusedPasswordHashesDomainModel.PasswordUniqueCount = _passwordConfiguration.UniqueCount;
			await _passwordHistoryDomainService.RemoveUnusedPasswordHistoriesAsync(removeUnusedPasswordHashesDomainModel);
			var lastUserPasswordHashes = await _passwordHistoryDomainService.GetLastUniquePasswordHashesAsync(getLastPasswordHashesDomainModel);
			if (lastUserPasswordHashes.Any(passwordHash => VerifyHashedPassword(user, passwordHash, model.Password)))
			{
				throw new NetKitAuthInputValidationException(ErrorCode.PasswordHashIsNotUnique, PasswordHashIsNotUniqueMessage);
			}
			var addPasswordHistoryDomainModel = Mapper.Map<AddPasswordHistoryDomainModel>(model);
			addPasswordHistoryDomainModel.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
			await _passwordHistoryDomainService.AddPasswordHistoryAsync(addPasswordHistoryDomainModel);
		}
		
		public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
		{
			var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
			return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
		}

		private async Task<string> HashPasswordAsync(string userId, string password)
		{
			var user = await _userDomainService.GetUserByIdAsync(userId);
			var hashedPassword = _passwordHasher.HashPassword(user, password);
			return hashedPassword;
		}
	}
}
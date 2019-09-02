// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using EnsureThat;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Softeq.NetKit.Auth.AppServices.Abstract;
using Softeq.NetKit.Auth.AppServices.TransportModels.User.Request;
using Softeq.NetKit.Auth.AppServices.TransportModels.User.Response;
using Softeq.NetKit.Auth.Common.Utility.AppleHttpClient.Interfaces;
using Softeq.NetKit.Auth.Domain.Models;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.Integration.Edc.Events;
using Softeq.NetKit.Auth.Integration.Edc.Services;

namespace Softeq.NetKit.Auth.AppServices.Services
{
    public class AppleService : BaseService, IAppleService
    {
        private readonly IUserService _userService;
        private readonly IEdcPublishService _edcPublishService;
        private readonly UserManager<User> _userManager;
        private readonly IAppleHttpClient _appleHttpClient;

        public AppleService(
            ILogger logger, 
            IMapper mapper, 
            IServiceProvider serviceProvider,
            IAppleHttpClient appleHttpClient,
            IUserService userService,
            IEdcPublishService edcPublishService,
            UserManager<User> userManager)
            : base(logger, mapper, serviceProvider)
        {
            _appleHttpClient = Ensure.Any.IsNotNull(appleHttpClient, nameof(appleHttpClient));
            _userService = Ensure.Any.IsNotNull(userService, nameof(userService));
            _edcPublishService = Ensure.Any.IsNotNull(edcPublishService, nameof(edcPublishService));
            _userManager = Ensure.Any.IsNotNull(userManager, nameof(userManager));
        }

        public async Task<UserResponse> GetUserAsync(AppleUserInformationModel userInformation)
        {
            User user;
            if (userInformation.Code != null && userInformation.Email != null)
            {
                if (await AppleUserConfirmation(userInformation))
                {
                    return null;
                }

                user = await _userManager.FindByEmailAsync(userInformation.Email) 
                       ?? await CreateUserAsync(userInformation);

                return Mapper.Map<User, UserResponse>(user);
            }

            user = await _userService.GetUserByAppleKeyAsync(userInformation.AppleKey);

            return user != null ? Mapper.Map<User, UserResponse>(user) : null;
        }



        private async Task<bool> AppleUserConfirmation(AppleUserInformationModel userInformation)
        {
            var response = await _appleHttpClient.UserConfirmationAsync(userInformation.Code);

            return response.StatusCode == HttpStatusCode.BadRequest;
        }

        private async Task<User> CreateUserAsync(AppleUserInformationModel userInformation)
        {
            var newUser = new User
            {
                UserName = userInformation.Email,
                Email = userInformation.Email,
                AppleKey = userInformation.AppleKey,
                EmailConfirmed = true,
                IsAcceptedTermsAndPolicy = true
            };

            await _userManager.CreateAsync(newUser, userInformation.Code);
            await _userManager.AddToRoleAsync(newUser, Roles.User);

            var createdUser = await _userManager.FindByEmailAsync(newUser.Email);

            await _edcPublishService.PublishAsync(new UserHasBeenRegisteredEvent(createdUser.Id, createdUser.Email));

            return createdUser;
        }
    }
}

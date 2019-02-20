// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Softeq.NetKit.Auth.AppServices.Abstract;
using Softeq.NetKit.Auth.AppServices.TransportModels;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Common.Exceptions.Response;
using Softeq.NetKit.Auth.Common.Utility.TokenProvider;
using Softeq.NetKit.Auth.Domain.Models;
using Softeq.NetKit.Auth.Domain.Models.Role;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Models;
using Softeq.NetKit.Auth.IdentityServer;
using Softeq.NetKit.Auth.Integration.Edc.Events;
using Softeq.NetKit.Auth.Integration.Edc.Services;
using Softeq.NetKit.Auth.Web.Models.Request;
using Softeq.NetKit.Auth.Web.Models.Response;
using Softeq.NetKit.Auth.Web.Utility.Attribute;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using IdentityError = Softeq.NetKit.Auth.Common.Exceptions.Response.IdentityError;

namespace Softeq.NetKit.Auth.Web.Controllers
{
    [Route("account")]
    public class AccountController : BaseController
    {
        private const string UserNotFoundException = "User does not exist.";
        private const string InvalidEmailConfirmationCodeException = "A code must be supplied for email confirmation.";
        private const string UserExistException = "User with email {0} is already exist.";
        private const string EmailConfirmedException = "Email is already activated.";
        private const string UserIsDeleted = "User is in deleted state.";
        private const string InactiveUserMessage = "User is in inactive state.";
        private const string UserIsPendingMessage = "User is in pending state.";

        private readonly IDictionary<int, ErrorResponseModel> _userStatusErrors = new Dictionary<int, ErrorResponseModel>
        {
            {(int) UserStatusEnum.Deleted, new ErrorResponseModel(ErrorCode.UserIsInDeletedState, UserIsDeleted)},
            {(int) UserStatusEnum.Inactive, new ErrorResponseModel(ErrorCode.InactiveUserState, InactiveUserMessage)},
            {(int) UserStatusEnum.Pending, new ErrorResponseModel(ErrorCode.UserIsInPendingState, UserIsPendingMessage)}
        };

        private readonly TokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly DefaultDataProtectorTokenProvider<User> _dataProtectorTokenProvider;
        private readonly IUserService _userService;
        private readonly IEdcPublishService _edcPublishService;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHistoryService _passwordHistoryService;

        public AccountController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger logger,
            IMapper mapper,
            IConfiguration configuration,
            TokenService tokenService,
            DefaultDataProtectorTokenProvider<User> dataProtectorTokenProvider,
            IUserService userService, IEdcPublishService edcPublishService,
            IPasswordHistoryService passwordHistoryService) : base(logger, mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _dataProtectorTokenProvider = dataProtectorTokenProvider;
            _userService = userService;
            _edcPublishService = edcPublishService;
            _passwordHistoryService = passwordHistoryService;
        }

        [Route("register")]
        [HttpPost]
        [ActionResponseType(typeof(void), HttpStatusCode.OK)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return Conflict(new ErrorResponseModel(ErrorCode.UserWithDefinedEmailAlreadyExist, string.Format(UserExistException, model.Email)));
            }

            var newUser = new User
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = false,
                IsAcceptedTermsAndPolicy = model.IsAcceptedTermsOfService
            };

            await _userManager.CreateAsync(newUser, model.Password);
            await _userManager.AddToRoleAsync(newUser, Roles.User);

            var createdUser = await _userManager.FindByEmailAsync(newUser.Email);
            await _userService.SendEmailConfirmationEmailAsync(createdUser);

            var integrationEvent = new UserHasBeenRegisteredEvent(newUser.Id, newUser.Email);
            await _edcPublishService.PublishAsync(integrationEvent);

            return Ok();
        }

        [Route("resend-confirmation-email")]
        [HttpPost]
        [ActionResponseType(typeof(void), HttpStatusCode.OK)]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequestModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound(new ErrorResponseModel(ErrorCode.UserNotFound, UserNotFoundException));
            }
            if (user.StatusId != (int)UserStatusEnum.Pending)
            {
                return Conflict(new ErrorResponseModel(ErrorCode.EmailAlreadyConfirmed, EmailConfirmedException));
            }
            
            await _userService.SendEmailConfirmationEmailAsync(user);

            return Ok();
        }

        [HttpGet]
        [Route("confirm-email")]
        [ActionResponseType(typeof(UserInfoResponseModel), HttpStatusCode.OK)]
        public async Task<IActionResult> ConfirmEmail(string code)
        {
            string userId;
            if (code == null)
            {
                return BadRequest(new ErrorResponseModel(ErrorCode.InvalidEmailConfirmationCodeException, InvalidEmailConfirmationCodeException));
            }

            try
            {
                userId = _dataProtectorTokenProvider.Validate(code);
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(new ErrorResponseModel(ErrorCode.ActivationCodeExpired, exception.Message));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ErrorResponseModel(ErrorCode.UserNotFound, UserNotFoundException));
            }

            await _userManager.ConfirmEmailAsync(user, code);

            await _userService.ConfirmEmailAsync(new BaseRequest(userId));

            return RedirectToAction(nameof(ConfirmedEmail));
        }

        [HttpGet]
        [Route("confirmed-email")]
        [ActionResponseType(typeof(ViewResult), HttpStatusCode.OK)]
        public ViewResult ConfirmedEmail()
        {
            return View();
        }

        [HttpPost]
        [Route("forgot-password")]
        [ActionResponseType(typeof(void), HttpStatusCode.OK)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound(new ErrorResponseModel(ErrorCode.UserNotFound, UserNotFoundException));
            }
            if (_userStatusErrors.ContainsKey(user.StatusId))
            {
                return Conflict(_userStatusErrors[user.StatusId]);
            }

            await _userService.SendResetPasswordEmailAsync(user);

            return Ok();
        }

        [HttpGet]
        [Route("reset-password")]
        [ActionResponseType(typeof(ViewResult), HttpStatusCode.OK)]
        public ViewResult ResetPassword(string code)
        {
            var resetPasswordModel = new ResetPasswordRequestModel
            {
                Code = code
            };

            return View(resetPasswordModel);
        }

        [Route("reset-password")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionResponseType(typeof(void), HttpStatusCode.OK)]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestModel model)
        {
            string userId;

            try
            {
                userId = _dataProtectorTokenProvider.Validate(model.Code);
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(new ErrorResponseModel(ErrorCode.ActivationCodeExpired, exception.Message));
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new ErrorResponseModel(ErrorCode.UserNotFound, UserNotFoundException));
            }

            if (_userStatusErrors.ContainsKey(user.StatusId))
            {
                return Conflict(_userStatusErrors[user.StatusId]);
            }

            var userPasswordDomainModel = new UserPasswordDomainModel
            {
                Password = model.Password,
                UserId = userId
            };

            await _passwordHistoryService.CheckAndAddPasswordHistoryAsync(userPasswordDomainModel);
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            await _userManager.SetLockoutEndDateAsync(user, null);
            await _userService.UpdateLastPasswordChangedDateAsync(userId);
            await _userService.UpdateLastPasswordExpiresEmailSentDateAsync(userId);
            await _tokenService.RevokeTokensAsync(user.Id, "client");

            return Ok();
        }

        [HttpGet]
        [Route("check-registration")]
        [ActionResponseType(typeof(void), HttpStatusCode.OK)]
        [ActionResponseType(typeof(void), HttpStatusCode.Conflict)]
        public async Task<IActionResult> CheckRegistration(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return Conflict();
            }

            return Ok();
        }

        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            var identityErrors = result.Errors?.Count() > 0 ?
                result.Errors.Select(err => new IdentityError(err.Code, err.Description)).ToList() :
                new List<IdentityError>();

            return BadRequest(new IdentityErrorResponseModel(ErrorCode.IdentityError, "reset password identity error", identityErrors));
        }
    }
}
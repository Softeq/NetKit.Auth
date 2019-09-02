// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using EnsureThat;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Softeq.NetKit.Auth.AppServices.Abstract;
using Softeq.NetKit.Auth.Common.Utility.Authorization;
using Softeq.NetKit.Auth.Web.Utility.Attribute;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Softeq.NetKit.Auth.AppServices.TransportModels.User.Request;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Common.Exceptions.Response;
using Softeq.NetKit.Auth.Web.Models.Request;

namespace Softeq.NetKit.Auth.Web.Controllers
{
    [Route("authorization")]
    public class AuthorizationController : BaseController
    {
        private const string UserNotFoundException = "User does not exist.";

        private readonly IUserService _userService;
        private readonly IAppleService _appleService;

        public AuthorizationController(
            ILogger logger, 
            IMapper mapper, 
            IUserService userService, 
            IAppleService appleService) 
            : base(logger, mapper)
        {
            _userService = Ensure.Any.IsNotNull(userService, nameof(userService));
            _appleService = Ensure.Any.IsNotNull(appleService, nameof(appleService));
        }

        [HttpGet]
        [Route("status")]
        [ActionResponseType(typeof(AuthorizationStatus), HttpStatusCode.OK)]
        public async Task<IActionResult> GetAuthorizationStatusAsync([FromQuery] string saasUserId)
        {
            var status = await _userService.GetAuthorizationStatusAsync(saasUserId);
            return Ok(status);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("apple")]
        [ActionResponseType(typeof(void), HttpStatusCode.OK)]
        public async Task<IActionResult> AppleAuth([FromForm] AppleRequestModel requestModel)
        {
            var user = await _appleService.GetUserAsync(
                Mapper.Map<AppleRequestModel, AppleUserInformationModel>(requestModel));

            if (user == null)
            {
                return NotFound(new ErrorResponseModel(ErrorCode.UserNotFound, UserNotFoundException));
            }

            return Ok();
        }
    }
}

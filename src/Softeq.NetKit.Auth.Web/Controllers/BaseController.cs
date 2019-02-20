// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net;
using System.Security.Claims;
using AutoMapper;
using Softeq.NetKit.Auth.Common.Exceptions.Response;
using Softeq.NetKit.Auth.Common.Utility;
using Softeq.NetKit.Auth.Web.Utility.Attribute;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Softeq.NetKit.Auth.Web.Controllers
{
    [Produces(MediaTypes.JsonContentType)]
    [ApiVersion("1.0")]
    [ActionResponseType(typeof(ErrorResponseModel), HttpStatusCode.BadRequest)]
    [ActionResponseType(typeof(ValidationErrorResponseModel), HttpStatusCode.BadRequest)]
    [ActionResponseType(typeof(ErrorResponseModel), HttpStatusCode.Unauthorized)]
    [ActionResponseType(typeof(ErrorResponseModel), HttpStatusCode.Forbidden)]
    [ActionResponseType(typeof(ErrorResponseModel), HttpStatusCode.NotFound)]
    [ActionResponseType(typeof(ErrorResponseModel), HttpStatusCode.Conflict)]
    [ActionResponseType(typeof(ErrorResponseModel), HttpStatusCode.InternalServerError)]
	public class BaseController : Controller
    {
        protected readonly ILogger Logger;
		protected readonly IMapper Mapper;

		public BaseController(ILogger logger, IMapper mapper)
        {
            Logger = logger;
            Mapper = mapper;
        }

        protected string GetCurrentUserName()
        {
            return User.FindFirstValue(JwtClaimTypes.Name);
        }

        protected string GetCurrentRole()
        {
            return User.FindFirstValue(JwtClaimTypes.Role);
        }

        protected string GetCurrentUserId()
        {
            return User.FindFirstValue(JwtClaimTypes.Subject);
        }
    }
}
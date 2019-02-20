// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Softeq.NetKit.Auth.AppServices.Abstract;
using Softeq.NetKit.Auth.Common.Utility.Authorization;
using Softeq.NetKit.Auth.Web.Utility.Attribute;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Softeq.NetKit.Auth.Web.Controllers
{
	[Route("authorization")]
    public class AuthorizationController : BaseController
	{
		private readonly IUserService _userService;

	    public AuthorizationController(ILogger logger, IMapper mapper, IUserService userService) : base(logger, mapper)
	    {
		    _userService = userService;
	    }

	    [HttpGet]
	    [Route("status")]
	    [ActionResponseType(typeof(AuthorizationStatus), HttpStatusCode.OK)]
	    public async Task<IActionResult> GetAuthorizationStatusAsync([FromQuery] string saasUserId)
	    {
		    var status = await _userService.GetAuthorizationStatusAsync(saasUserId);
		    return Ok(status);
	    }
	}
}

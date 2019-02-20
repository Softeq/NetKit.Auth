// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net;
using AutoMapper;
using Softeq.NetKit.Auth.Web.Models.Request;
using Softeq.NetKit.Auth.Web.Models.Response;
using Softeq.NetKit.Auth.Web.Utility.Attribute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Softeq.NetKit.Auth.Web.Controllers
{
	[Route("connect")]
    public class ConnectController : BaseController
    {
        public ConnectController(ILogger logger, IMapper mapper) 
            : base(logger, mapper)
        {
        }

        [AllowAnonymous]
	    [HttpPost]
	    [Route("token")]
	    [ActionResponseType(typeof(GenerateTokenResponseModel), HttpStatusCode.OK)]
	    public IActionResult DummyCreateTokenForSwagger([FromForm] GenerateTokenRequestModel requestModel)
        {
            return Ok();
        }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net;
using System.Reflection;
using AutoMapper;
using Softeq.NetKit.Auth.Web.Utility.Attribute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Softeq.NetKit.Auth.Web.Controllers
{
	[Route("version")]
	public class VersionController : BaseController
	{
		public VersionController(ILogger logger, IMapper mapper) : base(logger, mapper)
		{
		}

		[HttpGet]
		[AllowAnonymous]
		[ActionResponseType(typeof(string), HttpStatusCode.OK)]
		public IActionResult GetVersion()
		{
			var version = Assembly.GetExecutingAssembly().GetName().Version;

			return Ok($"{version.Major}.{version.Minor}.{version.Build}-{version.Revision}");
		}
	}
}
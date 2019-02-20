// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using AutoMapper;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Common.Utility.Helpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Softeq.NetKit.Auth.Web.Controllers
{
	[Route("errors")]
    public class ErrorsController: BaseController
    {
        public ErrorsController(ILogger logger, IMapper mapper) : base(logger, mapper)
        {
        }

        [HttpGet]
        [Route("codes")]
        public ActionResult<IDictionary<string, string>> ErrorCodes()
        {
            return Ok(EnumDictionary.Of<ErrorCode>());
        }
    }
}

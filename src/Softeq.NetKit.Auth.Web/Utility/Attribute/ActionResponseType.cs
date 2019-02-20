// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Softeq.NetKit.Auth.Web.Utility.Attribute
{
    public class ActionResponseType : ProducesResponseTypeAttribute
    {
	    public ActionResponseType(int statusCode) : base(statusCode)
	    {
	    }

	    public ActionResponseType(Type type, int statusCode) : base(type, statusCode)
	    {
	    }

	    public ActionResponseType(Type type, HttpStatusCode statusCode) : base(type, (int) statusCode)
	    {
	    }
	}
}

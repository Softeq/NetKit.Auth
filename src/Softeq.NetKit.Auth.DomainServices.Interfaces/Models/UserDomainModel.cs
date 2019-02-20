// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.NetKit.Auth.Domain.Models.User;

namespace Softeq.NetKit.Auth.DomainServices.Interfaces.Models
{
    public class UserDomainModel
    {
	    public string Id { get; set; }
	    public UserStatus Status { get; set; }
	    public DateTimeOffset Created { get; set; }
	    public string Email { get; set; }
	}
}

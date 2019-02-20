// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.NetKit.Auth.Domain.Models.UserRoles;
using Microsoft.AspNetCore.Identity;

namespace Softeq.NetKit.Auth.Domain.Models.Role
{
	public class Role : IdentityRole
    {
		public Role()
		{
		}

		public Role(string roleName): base(roleName)
		{
		}

		public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}

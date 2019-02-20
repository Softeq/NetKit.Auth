// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.AspNetCore.Identity;

namespace Softeq.NetKit.Auth.Domain.Models.UserRoles
{
    public class UserRole: IdentityUserRole<string>
	{
		public virtual Role.Role Role { get; set; }
		public virtual User.User User { get; set; }
    }
}

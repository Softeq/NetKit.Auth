// Developed by Softeq Development Corporation
// http://www.softeq.com
namespace Softeq.NetKit.Auth.Domain.Models
{
    public static class Roles
    {
        public const string User = "User";
        public const string Admin = "Admin";

	    public static bool IsAdminRole(string roleName)
	    {
		    return roleName == Admin;
	    }
	}
}
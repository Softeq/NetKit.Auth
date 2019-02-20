// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.Auth.Domain.Models.UserRoles;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;

namespace Softeq.NetKit.Auth.Repository.Interfaces.Repositories
{
	public interface IReadUserRolesRepository: IReadRepositoryWithoutKey<UserRole>
	{
		Task<UserRole> GetUserRoleByUserIdAsync(string userId);
	}
}
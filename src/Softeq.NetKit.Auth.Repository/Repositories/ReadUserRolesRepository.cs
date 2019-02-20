// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.Auth.Domain.Models.UserRoles;
using Softeq.NetKit.Auth.Repository.Interfaces.Repositories;
using Softeq.NetKit.Auth.SQLRepository;
using Microsoft.EntityFrameworkCore;
using Softeq.NetKit.Auth.Repository.Base;

namespace Softeq.NetKit.Auth.Repository.Repositories
{
	public class ReadUserRolesRepository: BaseReadRepositoryWithoutKey<UserRole>, IReadUserRolesRepository
	{
		public ReadUserRolesRepository(ApplicationDbContext dataContext) : base(dataContext)
		{
		}

		public async Task<UserRole> GetUserRoleByUserIdAsync(string userId)
		{
			return await dbset.Include(ur => ur.Role).Include(ur => ur.User).SingleAsync(ur => ur.UserId == userId);
		}
	}
}
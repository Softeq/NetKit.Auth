// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.Domain.Models;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.Repository.Interfaces.Repositories;
using Softeq.NetKit.Auth.SQLRepository;
using Microsoft.EntityFrameworkCore;
using Softeq.NetKit.Auth.Repository.Base;

namespace Softeq.NetKit.Auth.Repository.Repositories
{
    public class ReadUserRepository : BaseReadRepository<User, string>, IReadUserRepository
    {
        public ReadUserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await dbset.Include(x => x.Status)
                .SingleAsync(x => x.Id == userId);
        }

		public async Task<UserStatus> GetUserStatusByUserIdAsync(string userId)
		{
			return await dbset.Include(u => u.Status).Where(u => u.Id == userId).Select(u => u.Status).SingleAsync();
		}

	    public async Task<IEnumerable<User>> GetUsersWhosePasswordExpiresAsync(int expirationTimeInDays)
	    {
		    return await dbset.Where(u => u.UserRoles.Any(ur => Roles.IsAdminRole(ur.Role.Name)) &&
		                                  (!u.LastPasswordChangedDate.HasValue &&
		                                   (DateTimeOffset.UtcNow - u.Created).TotalDays >= expirationTimeInDays ||
		                                   u.LastPasswordChangedDate.HasValue &&
		                                   (DateTimeOffset.UtcNow - u.LastPasswordChangedDate).Value.TotalDays >=
		                                   expirationTimeInDays) &&
		                                  !u.LastPasswordExpiresEmailSentDate.HasValue)
			    .ToListAsync();
	    }

	    public async Task<User> GetUserByNameAsync(string userName)
	    {
		    return await dbset.Where(u => u.UserName == userName)
			    .SingleAsync();
	    }

        public async Task<User> GetFirstRoleRepresentativeAsync(string role)
        {
            return await dbset.Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                        .Where(u => u.UserRoles.Any(ur => ur.Role.Name == role)).OrderBy(u => u.Created).FirstOrDefaultAsync();
        }
    }
}

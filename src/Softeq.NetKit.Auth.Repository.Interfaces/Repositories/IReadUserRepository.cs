// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;

namespace Softeq.NetKit.Auth.Repository.Interfaces.Repositories
{
    public interface IReadUserRepository : IReadRepository<User, string>
    {
        Task<User> GetUserByIdAsync(string userId);
		Task<UserStatus> GetUserStatusByUserIdAsync(string userId);
	    Task<IEnumerable<User>> GetUsersWhosePasswordExpiresAsync(int expirationTimeInDays);
	    Task<User> GetUserByNameAsync(string userName);
        Task<User> GetFirstRoleRepresentativeAsync(string role);
    }
}

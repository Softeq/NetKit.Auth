// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;

namespace Softeq.NetKit.Auth.Repository.Interfaces.Repositories
{
	public interface IReadPasswordHistoryRepository : IReadRepository<PasswordHistory, Guid>
	{
		Task<IEnumerable<PasswordHistory>> GetUnusedPasswordHistoriesAsync(string userId, int passwordUniqueCount);
		Task<IEnumerable<string>> GetLastUniquePasswordHashesAsync(string userId, int passwordUniqueCount);
	}
}
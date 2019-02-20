// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.Repository.Interfaces.Repositories;
using Softeq.NetKit.Auth.SQLRepository;
using Microsoft.EntityFrameworkCore;
using Softeq.NetKit.Auth.Repository.Base;

namespace Softeq.NetKit.Auth.Repository.Repositories
{
	public class ReadPasswordHistoryRepository : BaseReadRepository<PasswordHistory, Guid>, IReadPasswordHistoryRepository
	{
		public ReadPasswordHistoryRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<PasswordHistory>> GetUnusedPasswordHistoriesAsync(string userId, int passwordUniqueCount)
		{
			return await dbset.Where(ph => ph.UserId == userId)
				.OrderByDescending(ph => ph.Created)
				.Skip(passwordUniqueCount)
				.ToListAsync();
		}

		public async Task<IEnumerable<string>> GetLastUniquePasswordHashesAsync(string userId, int passwordUniqueCount)
		{
			return await dbset.Where(ph => ph.UserId == userId)
				.OrderByDescending(ph => ph.Created)
				.Take(passwordUniqueCount)
				.Select(ph => ph.PasswordHash)
				.ToListAsync();
		}
	}
}
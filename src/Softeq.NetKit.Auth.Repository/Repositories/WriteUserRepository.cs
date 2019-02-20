// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.Repository.Base;
using Softeq.NetKit.Auth.Repository.Interfaces.Repositories;
using Softeq.NetKit.Auth.SQLRepository;

namespace Softeq.NetKit.Auth.Repository.Repositories
{
	public class WriteUserRepository : BaseWriteRepository<User>, IWriteUserRepository
	{
		public WriteUserRepository(ApplicationDbContext dataContext) : base(dataContext)
		{
		}
	}
}
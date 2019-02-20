// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;
using Softeq.NetKit.Auth.Repository.Interfaces;
using Softeq.NetKit.Auth.Repository.Interfaces.Repositories;
using Softeq.NetKit.Auth.SQLRepository;
using Transaction = Softeq.NetKit.Auth.SQLRepository.Transaction;

namespace Softeq.NetKit.Auth.Repository
{
    public class AuthUnitOfWork : IAuthUnitOfWork, IDisposable
    {
        protected ApplicationDbContext DbContext;

        public AuthUnitOfWork(
            ApplicationDbContext context,
            IReadUserRepository readUserRepository,
            IWriteUserRepository writeUserRepository,
            IReadUserRolesRepository readUserRolesRepository,
			IReadPasswordHistoryRepository readPasswordInfoRepository, 
			IWritePasswordHistoryRepository writePasswordInfoRepository)
        {
            DbContext = context;
            ReadUserRepository = readUserRepository;
            WriteUserRepository = writeUserRepository;
            ReadUserRolesRepository = readUserRolesRepository;
	        ReadPasswordHistoryRepository = readPasswordInfoRepository;
	        WritePasswordHistoryRepository = writePasswordInfoRepository;
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync(true);
        }

        public ITransaction BeginTransaction()
        {
            return new Transaction(DbContext);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }

        #region Repositories

        public IReadUserRepository ReadUserRepository { get; }
        public IReadUserRolesRepository ReadUserRolesRepository { get; }
        public IReadPasswordHistoryRepository ReadPasswordHistoryRepository { get; }
		public IWriteUserRepository WriteUserRepository { get; }
        public IWritePasswordHistoryRepository WritePasswordHistoryRepository { get; }

		#endregion
	}
}
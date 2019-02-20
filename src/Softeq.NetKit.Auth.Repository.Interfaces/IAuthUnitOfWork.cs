// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;
using Softeq.NetKit.Auth.Repository.Interfaces.Repositories;

namespace Softeq.NetKit.Auth.Repository.Interfaces
{
    public interface IAuthUnitOfWork : IUnitOfWork
    {
        IReadUserRepository ReadUserRepository { get; }
        IReadUserRolesRepository ReadUserRolesRepository { get; }
		IReadPasswordHistoryRepository ReadPasswordHistoryRepository { get; }
        IWriteUserRepository WriteUserRepository { get; }
        IWritePasswordHistoryRepository WritePasswordHistoryRepository { get; }
	}
}

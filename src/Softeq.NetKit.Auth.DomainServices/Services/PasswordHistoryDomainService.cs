// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Interfaces;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Models;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;
using Softeq.NetKit.Auth.Repository.Interfaces;

namespace Softeq.NetKit.Auth.DomainServices.Services
{
	public class PasswordHistoryDomainService : BaseDomainService<IAuthUnitOfWork>, IPasswordHistoryDomainService
	{
		public PasswordHistoryDomainService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{
		}

		public Task AddPasswordHistoryAsync(AddPasswordHistoryDomainModel model)
		{
			var passwordInfo = Mapper.Map<PasswordHistory>(model);

			UnitOfWork.WritePasswordHistoryRepository.Add(passwordInfo);
            return UnitOfWork.SaveChangesAsync();
        }

		public async Task RemoveUnusedPasswordHistoriesAsync(RemoveUnusedPasswordHashesDomainModel model)
		{
			var passwordHistories = await UnitOfWork.ReadPasswordHistoryRepository.GetUnusedPasswordHistoriesAsync(model.UserId,
				model.PasswordUniqueCount);

			foreach (var passwordHistory in passwordHistories)
			{
				UnitOfWork.WritePasswordHistoryRepository.Delete(passwordHistory);
				await UnitOfWork.SaveChangesAsync();
			}
		}

		public Task<IEnumerable<string>> GetLastUniquePasswordHashesAsync(GetLastPasswordHashesDomainModel model)
		{
			return UnitOfWork.ReadPasswordHistoryRepository.GetLastUniquePasswordHashesAsync(model.UserId,
				model.PasswordUniqueCount);
		}
	}
}
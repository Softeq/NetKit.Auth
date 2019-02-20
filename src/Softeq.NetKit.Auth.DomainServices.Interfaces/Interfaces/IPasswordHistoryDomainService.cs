// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Models;

namespace Softeq.NetKit.Auth.DomainServices.Interfaces.Interfaces
{
	public interface IPasswordHistoryDomainService
	{
		Task AddPasswordHistoryAsync(AddPasswordHistoryDomainModel model);
		Task RemoveUnusedPasswordHistoriesAsync(RemoveUnusedPasswordHashesDomainModel model);
		Task<IEnumerable<string>> GetLastUniquePasswordHashesAsync(GetLastPasswordHashesDomainModel model);
	}
}
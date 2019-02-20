// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Models;

namespace Softeq.NetKit.Auth.AppServices.Abstract
{
	public interface IPasswordHistoryService
	{
		Task AddPasswordHistoryAsync(UserPasswordDomainModel model);
		Task CheckAndAddPasswordHistoryAsync(UserPasswordDomainModel model);
		bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
	}
}
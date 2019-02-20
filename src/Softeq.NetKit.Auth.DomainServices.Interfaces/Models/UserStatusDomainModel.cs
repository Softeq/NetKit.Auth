// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Domain.Models.User;

namespace Softeq.NetKit.Auth.DomainServices.Interfaces.Models
{
	public class UserStatusDomainModel
    {
		public UserStatusDomainModel()
		{
		}

		public UserStatusDomainModel(UserStatusEnum statusEnum)
		{
			Id = (int)statusEnum;
			Name = statusEnum.ToString();
		}

		public int Id { get; set; }
		public string Name { get; set; }
	}
}

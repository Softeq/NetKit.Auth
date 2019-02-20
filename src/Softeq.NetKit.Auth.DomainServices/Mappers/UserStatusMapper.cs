// Developed by Softeq Development Corporation
// http://www.softeq.com

using AutoMapper;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Models;

namespace Softeq.NetKit.Auth.DomainServices.Mappers
{
	public class UserStatusMapper : Profile
	{
		public UserStatusMapper()
		{
			CreateMap<UserStatus, UserStatusDomainModel>()
				.ForMember(x => x.Id, cfg => cfg.MapFrom(x => x.Id))
				.ForMember(x => x.Name, cfg => cfg.MapFrom(x => x.Name));
		}
	}
}

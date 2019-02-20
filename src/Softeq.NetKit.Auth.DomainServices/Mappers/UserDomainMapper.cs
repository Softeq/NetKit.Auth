// Developed by Softeq Development Corporation
// http://www.softeq.com

using AutoMapper;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Models;

namespace Softeq.NetKit.Auth.DomainServices.Mappers
{
	public class UserDomainMapper : Profile
	{
		public UserDomainMapper()
		{
			CreateMap<User, UserDomainModel>()
				.ForMember(x => x.Id, cfg => cfg.MapFrom(x => x.Id))
				.ForMember(x => x.Status, cfg => cfg.MapFrom(x => x.Status))
				.ForMember(x => x.Email, cfg => cfg.MapFrom(x => x.Email))
				.ForMember(x => x.Created, cfg => cfg.MapFrom(x => x.Created));

			CreateMap<UpdateUserDomainModel, User>()
				.ForMember(x => x.Status, cfg => cfg.Ignore())
				.ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.Email))
				.ForMember(x => x.NormalizedEmail, cfg => cfg.MapFrom(x => x.Email.ToUpperInvariant()))
				.ForMember(x => x.NormalizedUserName, cfg => cfg.MapFrom(x => x.Email.ToUpperInvariant()))
				.ForMember(x => x.Email, cfg => cfg.MapFrom(x => x.Email));
		}
	}
}

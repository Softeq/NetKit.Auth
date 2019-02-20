// Developed by Softeq Development Corporation
// http://www.softeq.com

using AutoMapper;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Models;

namespace Softeq.NetKit.Auth.DomainServices.Mappers
{
	public class PasswordHistoryDomainMapper : Profile
	{
		public PasswordHistoryDomainMapper()
		{
			CreateMap<AddPasswordHistoryDomainModel, PasswordHistory>()
				.ForMember(ph => ph.UserId, cfg => cfg.MapFrom(m => m.UserId))
				.ForMember(ph => ph.PasswordHash, cfg => cfg.MapFrom(m => m.PasswordHash));

			CreateMap<UserPasswordDomainModel, GetLastPasswordHashesDomainModel>()
				.ForMember(glp => glp.UserId, cfg => cfg.MapFrom(m => m.UserId));

			CreateMap<UserPasswordDomainModel, RemoveUnusedPasswordHashesDomainModel>()
				.ForMember(m => m.UserId, cfg => cfg.MapFrom(m => m.UserId));

		    CreateMap<GetLastPasswordHashesDomainModel, AddPasswordHistoryDomainModel>()
		        .ForMember(m => m.UserId, cfg => cfg.MapFrom(m => m.UserId));

		    CreateMap<UserPasswordDomainModel, AddPasswordHistoryDomainModel>()
		        .ForMember(m => m.PasswordHash, cfg => cfg.Ignore())
		        .ForMember(m => m.Created, cfg => cfg.Ignore());
		}
	}
}
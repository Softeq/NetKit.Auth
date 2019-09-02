// Developed by Softeq Development Corporation
// http://www.softeq.com

using AutoMapper;
using Softeq.NetKit.Auth.AppServices.TransportModels.User.Response;
using Softeq.NetKit.Auth.Domain.Models.User;

namespace Softeq.NetKit.Auth.AppServices.Mappers
{
    public class UserResponseMapper : Profile
    {
        public UserResponseMapper()
        {
            CreateMap<User, UserResponse>()
                .ForMember(x => x.Email, cfg => cfg.MapFrom(x => x.Email))
                .ForMember(x => x.Created, cfg => cfg.MapFrom(x => x.Created))
                .ForMember(x => x.UserStatus, cfg => cfg.MapFrom(x => x.StatusId))
                .ForMember(x => x.Id, cfg => cfg.MapFrom(x => x.Id));
        }
    }
}

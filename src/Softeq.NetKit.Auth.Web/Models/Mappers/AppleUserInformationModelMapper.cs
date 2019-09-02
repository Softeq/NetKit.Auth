// Developed by Softeq Development Corporation
// http://www.softeq.com

using AutoMapper;
using Softeq.NetKit.Auth.AppServices.TransportModels.User.Request;
using Softeq.NetKit.Auth.Web.Models.Request;

namespace Softeq.NetKit.Auth.Web.Models.Mappers
{
    public class AppleUserInformationModelMapper : Profile
    {
        public AppleUserInformationModelMapper()
        {
            CreateMap<AppleRequestModel, AppleUserInformationModel>()
                .ForMember(x => x.Email, cfg => cfg.MapFrom(x => x.Email))
                .ForMember(x => x.Code, cfg => cfg.MapFrom(x => x.Code))
                .ForMember(x => x.AppleKey, cfg => cfg.MapFrom(x => x.AppleKey));
        }
    }
}

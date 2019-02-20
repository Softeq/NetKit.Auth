// Developed by Softeq Development Corporation
// http://www.softeq.com
namespace Softeq.NetKit.Auth.AppServices.TransportModels.Shared
{
    public class IdentityDto<TIdentity>
    {
        public IdentityDto()
        { }
        public IdentityDto(TIdentity id)
        {
            Id = id;
        }
        public TIdentity Id { get; set; }
    }
}

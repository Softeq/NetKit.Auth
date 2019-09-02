// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.Auth.AppServices.TransportModels.User.Request;
using Softeq.NetKit.Auth.AppServices.TransportModels.User.Response;

namespace Softeq.NetKit.Auth.AppServices.Abstract
{
    public interface IAppleService
    {
       Task<UserResponse> GetUserAsync(AppleUserInformationModel userInformation);
    }
}

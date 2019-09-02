// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;
using System.Threading.Tasks;

namespace Softeq.NetKit.Auth.Common.Utility.AppleHttpClient.Interfaces
{
    public interface IAppleHttpClient
    {
        Task<HttpResponseMessage> UserConfirmationAsync(string code);
    }
}

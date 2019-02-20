// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.NetKit.Auth.Web.Utility.DbInitializer
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }
}

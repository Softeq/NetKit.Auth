// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Web.Models.Request
{
    public interface IConfirmPasswordModel
    {
        string Password { get; set; }

        string ConfirmPassword { get; set; }
    }
}

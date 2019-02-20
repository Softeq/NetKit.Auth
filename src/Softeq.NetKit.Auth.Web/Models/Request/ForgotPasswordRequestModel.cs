// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Web.Models.Request
{
    public class ForgotPasswordRequestModel : IEmailModel
    {
        public string Email { get; set; }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Web.Models.Request
{
    public class RegisterUserRequestModel : IEmailModel, IPasswordModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsAcceptedTermsOfService { get; set; }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Web.Models.Request
{
    public class ResetPasswordRequestModel : IPasswordModel, IConfirmPasswordModel, ICodeModel
    {
        public string Password { get; set; }

        public string ConfirmPassword{ get; set; }

        public string Code { get; set; }
    }
}
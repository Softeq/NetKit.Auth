// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Web.Models.Request
{
    public class AppleRequestModel : IEmailModel
    {
        public string Code { get; set; }
        public string AppleKey { get; set; }
        public string Email { get; set; }
    }
}

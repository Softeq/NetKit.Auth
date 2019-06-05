// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Web.Models.Request
{
    public class GenerateTokenRequestModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Scope { get; set; }

        public string Grant_type { get; set; }

        public string Client_id { get; set; }

        public string Client_secret { get; set; }

        public string Refresh_token { get; set; }
    }
}
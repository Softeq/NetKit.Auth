// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Web.Models.Response
{
    public class GenerateTokenResponseModel
    {
		public string Access_token { get; set; }

		public int Expires_in { get; set; }

		public string Token_type { get; set; }

		public string Refresh_token { get; set; }
    }
}

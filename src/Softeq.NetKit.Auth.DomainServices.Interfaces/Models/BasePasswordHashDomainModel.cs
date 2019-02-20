// Developed by Softeq Development Corporation
// http://www.softeq.com
namespace Softeq.NetKit.Auth.DomainServices.Interfaces.Models
{
	public class BasePasswordHashDomainModel
	{
		public string UserId { get; set; }
		public int PasswordUniqueCount { get; set; }
	}
}
// Developed by Softeq Development Corporation
// http://www.softeq.com
namespace Softeq.NetKit.Auth.AppServices.Utility
{
	public class PasswordConfiguration
	{
		public int UniqueCount { get; set; }
		public int ActivePeriodInDays { get; set; }
		public int TokenLifeTimeInMinutes { get; set; }
	}
}
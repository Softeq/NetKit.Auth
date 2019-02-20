// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.DomainServices.Interfaces.Models
{
	public class AddPasswordHistoryDomainModel
	{
		public string UserId { get; set; }
		public string PasswordHash { get; set; }
		public DateTimeOffset Created { get; set; }
	}
}
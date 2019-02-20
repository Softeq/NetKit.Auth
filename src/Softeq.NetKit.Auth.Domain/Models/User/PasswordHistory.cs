// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;

namespace Softeq.NetKit.Auth.Domain.Models.User
{
	public class PasswordHistory : Entity<Guid>, ICreated
	{
		public string UserId { get; set; }
		public virtual User User { get; set; }
		public string PasswordHash { get; set; }
		public DateTimeOffset Created { get; set; }
	}
}
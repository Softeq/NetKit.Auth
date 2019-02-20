// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Softeq.NetKit.Auth.Common.Utility.Hashing
{
	public class DefaultPasswordHasher<TUser> : PasswordHasher<TUser> where TUser : class
	{
		public DefaultPasswordHasher(IOptions<DefaultPasswordHasherOptions> options) : base(options)
		{
		}
	}

	public class DefaultPasswordHasherOptions : PasswordHasherOptions { }
}
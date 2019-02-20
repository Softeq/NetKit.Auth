// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.NetKit.Auth.Domain.Models.UserRoles;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Softeq.NetKit.Auth.Domain.Models.User
{
    public class User : IdentityUser, IBaseEntity<string>, ICreated
    {
        public override string Email { get; set; }
		public int StatusId { get; set; }
	    public virtual UserStatus Status { get; set; }
		public override string Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool IsAcceptedTermsAndPolicy { get; set; }
        public DateTimeOffset? TokenRevokedDate { get; set; }
		public DateTimeOffset? LastPasswordChangedDate { get; set; }
		public DateTimeOffset? LastPasswordExpiredEmailSentDate { get; set; }
	    public DateTimeOffset? LastPasswordExpiresEmailSentDate { get; set; }
		public DateTimeOffset? LastAccountFailedAttemptsEmailSentDate { get; set; }
		public virtual ICollection<UserRole> UserRoles { get; set; }
		public virtual ICollection<PasswordHistory> Passwords { get; set; }
        public virtual DeletedUserInfo DeletedUserInfo { get; set; }
    }
}

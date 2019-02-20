// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.NetKit.Auth.Domain.Models.User
{
    public class UserStatus : Entity<int>
    {
	    public UserStatus()
	    {
	    }

	    public UserStatus(UserStatusEnum userStatus)
	    {
		    Id = (int)userStatus;
		    Name = userStatus.ToString();
	    }

	    public string Name { get; set; }

	    public virtual ICollection<User> Users { get; set; }

	    public static implicit operator UserStatusEnum(UserStatus userStatus) => (UserStatusEnum)userStatus.Id;
	    public static implicit operator UserStatus(UserStatusEnum userStatus) => new UserStatus(userStatus);
	}
}

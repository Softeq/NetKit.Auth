// Developed by Softeq Development Corporation
// http://www.softeq.com
namespace Softeq.NetKit.Auth.Domain.Models.User
{
    public class DeletedUserInfo
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string NormalizedEmail { get; set; }

        public virtual User User { get; set; }
    }
}

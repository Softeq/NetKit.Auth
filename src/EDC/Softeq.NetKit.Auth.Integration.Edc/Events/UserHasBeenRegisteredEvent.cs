// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Integration.Edc.Events
{
    public class UserHasBeenRegisteredEvent : BaseEdcEvent
    {
        public UserHasBeenRegisteredEvent(string userId, string email)
        {
            UserId = userId;
            Email = email;
        }

        public string UserId { get; }

        public string Email { get; }

    }
}

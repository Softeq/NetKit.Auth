// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.AppServices.TransportModels.User.Response
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserStatus { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Email { get; set; }
    }
}

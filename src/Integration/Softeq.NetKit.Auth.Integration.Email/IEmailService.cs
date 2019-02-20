// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.Auth.Integration.Email.Dto;
using Softeq.NetKit.Auth.Integration.Email.Notifications;

namespace Softeq.NetKit.Auth.Integration.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(SendEmailDto email);

        Task SendNotificationAsync(IEmailNotification emailNotification);
    }
}

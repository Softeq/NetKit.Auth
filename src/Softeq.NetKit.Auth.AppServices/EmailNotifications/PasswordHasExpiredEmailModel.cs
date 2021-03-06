﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Integration.Email.Notifications;

namespace Softeq.NetKit.Auth.AppServices.EmailNotifications
{
	public class PasswordHasExpiredEmailModel : IEmailNotificationTemplateModel
    {
		public PasswordHasExpiredEmailModel(string link, string name)
		{
			Link = link;
			Name = name;
		}

		public string Link { get; set; }
		public string Name { get; set; }
	}
}
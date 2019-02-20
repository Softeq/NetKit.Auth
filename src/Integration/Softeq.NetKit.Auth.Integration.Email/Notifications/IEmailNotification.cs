// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.NetKit.Auth.Integration.Email.Dto;

namespace Softeq.NetKit.Auth.Integration.Email.Notifications
{
    public interface IEmailNotification
    {
        string Subject { get; set; }

        IEnumerable<RecipientDto> Recipients { get; set; }

        string Text { get; set; }

        string BaseHtmlTemplate { get; set; }

        string HtmlTemplate { get; set; }

		string GetHtml();
	}
}

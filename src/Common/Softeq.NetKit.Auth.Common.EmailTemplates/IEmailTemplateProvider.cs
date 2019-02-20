// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Common.EmailTemplates
{
    public interface IEmailTemplateProvider
    {
        string GetBaseTemplateHtml();

        string GetTemplateHtml(string templateName);
    }
}
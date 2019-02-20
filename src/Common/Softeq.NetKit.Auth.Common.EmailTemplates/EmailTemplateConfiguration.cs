// Developed by Softeq Development Corporation
// http://www.softeq.com

using EnsureThat;

namespace Softeq.NetKit.Auth.Common.EmailTemplates
{
    public class EmailTemplateConfiguration
    {
        public EmailTemplateConfiguration(string templatesFolderPath)
        {
            Ensure.That(templatesFolderPath).IsNotNullOrWhiteSpace();

            TemplatesFolderPath = templatesFolderPath;
        }

        public string TemplatesFolderPath { get; }
    }
}
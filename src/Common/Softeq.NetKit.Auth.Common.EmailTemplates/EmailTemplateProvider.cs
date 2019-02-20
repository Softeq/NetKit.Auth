// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Linq;
using EnsureThat;
using Softeq.Serilog.Extension;
using WebMarkupMin.Core;
using ILogger = Serilog.ILogger;

namespace Softeq.NetKit.Auth.Common.EmailTemplates
{
    internal class EmailTemplateProvider : IEmailTemplateProvider
    {
        private readonly IMarkupMinifier _markupMinifier;
        private readonly ILogger _logger;
        private readonly EmailTemplateConfiguration _emailTemplateConfiguration;

        public EmailTemplateProvider(IMarkupMinifier markupMinifier,
            ILogger logger,
            EmailTemplateConfiguration emailTemplateConfiguration)
        {
            Ensure.That(markupMinifier).IsNotNull();
            Ensure.That(logger).IsNotNull();
            Ensure.That(emailTemplateConfiguration).IsNotNull();

            _markupMinifier = markupMinifier;
            _logger = logger;
            _emailTemplateConfiguration = emailTemplateConfiguration;
        }

        public string GetBaseTemplateHtml()
        {
            return GetTemplateHtml("BaseTemplate");
        }

        public string GetTemplateHtml(string templateName)
        {
            var html = File.ReadAllText($"{_emailTemplateConfiguration.TemplatesFolderPath}/{templateName}.html");

            var minificationResult = _markupMinifier.Minify(html);

            if (minificationResult.Errors.Any())
            {
                var errors = minificationResult.Errors.Select(error => error.Message).ToList();

                _logger.Event("Softeq.NetKit.EmailTemplateProvider")
                    .With.Message($"Couldn't minify content for '{templateName}' template. Errors: {string.Join(";", errors)}")
                    .AsError();

                return html;
            }

            return minificationResult.MinifiedContent;
        }
    }
}
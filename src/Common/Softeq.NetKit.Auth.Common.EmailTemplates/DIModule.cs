// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using WebMarkupMin.Core;

namespace Softeq.NetKit.Auth.Common.EmailTemplates
{
    public class DIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HtmlMinifier>().As<IMarkupMinifier>();

            builder.RegisterType<EmailTemplateProvider>().As<IEmailTemplateProvider>();
        }
    }
}
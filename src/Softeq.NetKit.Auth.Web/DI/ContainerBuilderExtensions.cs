// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using IntegrationEdcModule = Softeq.NetKit.Auth.Integration.Edc.DIModule;
using CommonLoggerModule = Softeq.NetKit.Auth.Common.Logger.DIModule;
using IntegrationEmailModule = Softeq.NetKit.Auth.Integration.Email.DIModule;
using CommonEmailTemplatesModule = Softeq.NetKit.Auth.Common.EmailTemplates.DIModule;

namespace Softeq.NetKit.Auth.Web.DI
{
	internal static class ContainerBuilderExtensions
	{
		public static void RegisterSolutionModules(this ContainerBuilder containerBuilder)
		{
		    containerBuilder.RegisterAssemblyModules(typeof(Startup).Assembly);

            containerBuilder.RegisterModule<IntegrationEdcModule>();
		    containerBuilder.RegisterModule<CommonLoggerModule>();
		    containerBuilder.RegisterModule<IntegrationEmailModule>();
            containerBuilder.RegisterModule<CommonEmailTemplatesModule>();
        }
	}
}
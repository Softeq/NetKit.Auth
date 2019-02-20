// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;

namespace Softeq.NetKit.Auth.Integration.Email
{
	public class DIModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
		    builder.RegisterType<EmailService>().As<IEmailService>();
		}
	}
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;
using Softeq.NetKit.Auth.Repository;
using Softeq.NetKit.Auth.Repository.Interfaces;
using Softeq.NetKit.Auth.Repository.Interfaces.Repositories;
using Softeq.NetKit.Auth.Repository.Repositories;
using Softeq.NetKit.Auth.Web.Models.Validators;
using Softeq.NetKit.Auth.Web.Utility.DbInitializer;

namespace Softeq.NetKit.Auth.Web.DI
{
    public class DbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthUnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<AuthUnitOfWork>().As<IAuthUnitOfWork>();

            builder.RegisterType<DatabaseInitializer>().As<IDatabaseInitializer>();

            builder.RegisterType<ReadUserRepository>().As<IReadUserRepository>();
			builder.RegisterType<ReadUserRolesRepository>().As<IReadUserRolesRepository>();
			builder.RegisterType<ReadPasswordHistoryRepository>().As<IReadPasswordHistoryRepository>();
			builder.RegisterType<WriteUserRepository>().As<IWriteUserRepository>();
			builder.RegisterType<WritePasswordHistoryRepository>().As<IWritePasswordHistoryRepository>();

			builder.RegisterType<EmailValidator>().AsSelf();
			builder.RegisterType<PasswordValidator>().AsSelf();
            builder.RegisterType<ConfirmPasswordValidator>().AsSelf();
			builder.RegisterType<CodeValidator>().AsSelf();
		}
    }
}
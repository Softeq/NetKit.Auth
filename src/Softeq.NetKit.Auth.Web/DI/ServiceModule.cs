// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using Softeq.NetKit.Auth.AppServices.Abstract;
using Softeq.NetKit.Auth.AppServices.Services;
using Softeq.NetKit.Auth.Common.Utility.Authorization;
using Softeq.NetKit.Auth.DomainServices.Interfaces.Interfaces;
using Softeq.NetKit.Auth.DomainServices.Services;
using Softeq.NetKit.Auth.IdentityServer;

namespace Softeq.NetKit.Auth.Web.DI
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region AppServices

            builder.RegisterType<UserService>().As<IUserService>();
	        builder.RegisterType<AuthorizationStatusStatusValidator>().As<IAuthorizationStatusValidator>();
	        builder.RegisterType<TokenProviderService>().As<ITokenProviderService>();
	        builder.RegisterType<PasswordHistoryService>().As<IPasswordHistoryService>();

            #endregion

            #region DomainServices

            builder.RegisterType<UserDomainService>().As<IUserDomainService>();
            builder.RegisterType<PasswordHistoryDomainService>().As<IPasswordHistoryDomainService>();

			#endregion

			builder.RegisterType<TokenService>().AsSelf();
            builder.RegisterType<AppleService>().As<IAppleService>();
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using Softeq.NetKit.Auth.Common.Utility.AppleHttpClient.Interfaces;

namespace Softeq.NetKit.Auth.Common.Utility.AppleHttpClient.Extensions.DependancyInjection
{
    public static class AppleHttpClientContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterAppleHttpClient(this ContainerBuilder builder)
        {
            builder.RegisterType<AppleHttpClient>().As<IAppleHttpClient>();

            return builder;
        }
    }
}

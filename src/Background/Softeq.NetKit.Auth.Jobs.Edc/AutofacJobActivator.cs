// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using EnsureThat;
using Microsoft.Azure.WebJobs.Host;

namespace Softeq.NetKit.Auth.Jobs.Edc
{
    public class AutofacJobActivator : IJobActivator
    {
        private readonly IComponentContext _container;

        public AutofacJobActivator(IComponentContext container)
        {
            Ensure.That(container).IsNotNull();

            _container = container;
        }

        public T CreateInstance<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
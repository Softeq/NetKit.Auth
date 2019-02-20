// Developed by Softeq Development Corporation
// http://www.softeq.com

using EnsureThat;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace Softeq.NetKit.Auth.Jobs.Edc
{
    public class ConfigExpressionNameResolver : INameResolver
    {
        private readonly IConfiguration _configuration;

        public ConfigExpressionNameResolver(IConfiguration configuration)
        {
            Ensure.That(configuration).IsNotNull();

            _configuration = configuration;
        }

        public string Resolve(string name)
        {
            return _configuration[name];
        }
    }
}
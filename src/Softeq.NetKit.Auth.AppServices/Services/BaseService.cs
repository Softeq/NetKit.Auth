// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using AutoMapper;
using EnsureThat;
using Serilog;

namespace Softeq.NetKit.Auth.AppServices.Services
{
    public class BaseService
    {
        protected readonly ILogger Log;

        protected readonly IMapper Mapper;

        protected readonly IServiceProvider ServiceProvider;

        protected BaseService(ILogger logger, IMapper mapper, IServiceProvider serviceProvider)
        {
            Ensure.That(logger, nameof(logger)).IsNotNull();
            Ensure.That(serviceProvider, nameof(serviceProvider)).IsNotNull();
            Ensure.That(mapper, nameof(mapper)).IsNotNull();

            Log = logger;
            ServiceProvider = serviceProvider;
            Mapper = mapper;
        }
    }
}
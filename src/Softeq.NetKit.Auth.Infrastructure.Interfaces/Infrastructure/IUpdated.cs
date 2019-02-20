// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure
{
    public interface IUpdated
    {
        DateTimeOffset? Updated { get; set; }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
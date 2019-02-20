// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure
{
    public class Disposable : IDisposable
    {
        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Disposable()
        {
            Dispose(false);
        }

        protected virtual void DisposeCore()
        {
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
                DisposeCore();

            _isDisposed = true;
        }
    }
}
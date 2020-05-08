using System;

namespace Dot.Core.Dispose
{
    public abstract class ADisposable : IDisposable
    {
        private bool disposed = false;

        ~ADisposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                DisposeManagedResource();
            }

            DisposeUnmanagedResource();
            disposed = true;
        }
        //清理托管资源
        protected abstract void DisposeManagedResource();
        //清理非托管资源
        protected abstract void DisposeUnmanagedResource();
    }
}

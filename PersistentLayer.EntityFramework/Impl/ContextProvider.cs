using System;
using System.Data.Entity;

namespace PersistentLayer.EntityFramework.Impl
{
    public class ContextProvider
        : IContextProvider
    {
        private DbContext context;
        private bool isDisposed;

        public ContextProvider(DbContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            this.EnsureDisposed();
            this.isDisposed = true;

            try
            {
                this.context.Dispose();
            }
            catch (Exception)
            {
            }
        }

        public DbContext GetCurrentContext()
        {
            this.EnsureDisposed();
            return this.context;
        }

        private void EnsureDisposed()
        {
            if (this.isDisposed)
                throw new ObjectDisposedException("the current ContextProvider was disposed.");
        }
    }
}

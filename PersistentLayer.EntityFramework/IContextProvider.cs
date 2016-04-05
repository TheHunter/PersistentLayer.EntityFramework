using System;
using System.Data.Entity;

namespace PersistentLayer.EntityFramework
{
    public interface IContextProvider
        : IDisposable
    {
        /// <summary>
        /// Gets the current context.
        /// </summary>
        /// <returns></returns>
        DbContext GetCurrentContext();
    }
}

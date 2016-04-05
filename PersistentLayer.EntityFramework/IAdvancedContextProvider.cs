using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace PersistentLayer.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="PersistentLayer.ITransactionProvider" />
    public interface IAdvancedContextProvider
        : IFutureQueryBatch
    {
        IContextProvider ContextProvider { get; }
    }
}

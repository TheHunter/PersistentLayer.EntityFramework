namespace PersistentLayer.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="PersistentLayer.ITransactionProvider" />
    public interface IFutureContextProvider
        : IFutureQueryBatch
    {
        IContextProvider ContextProvider { get; }
    }
}

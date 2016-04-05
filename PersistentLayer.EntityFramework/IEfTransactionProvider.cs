namespace PersistentLayer.EntityFramework
{
    public interface IEfTransactionProvider
        : ITransactionProvider
    {
        IContextProvider ContextProvider { get; }
    }
}

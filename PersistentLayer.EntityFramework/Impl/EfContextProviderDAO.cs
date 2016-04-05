namespace PersistentLayer.EntityFramework.Impl
{
    public class EfContextProviderDAO
    {
        public EfContextProviderDAO(IEfTransactionProvider transactionProvider)
        {
            this.TransactionProvider = transactionProvider;
            this.FutureContextProvider = new FutureContextProvider(transactionProvider.ContextProvider);
        }

        public IEfTransactionProvider TransactionProvider { get; }

        public IFutureContextProvider FutureContextProvider { get; }
    }
}

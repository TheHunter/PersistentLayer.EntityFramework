namespace PersistentLayer.EntityFramework.Impl
{
    public class EfContextProviderDAO
    {
        public EfContextProviderDAO(IEfTransactionProvider contextProvider)
        {
            this.ContextProvider = contextProvider;
        }

        public IEfTransactionProvider ContextProvider { get; }
    }
}

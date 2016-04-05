using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace PersistentLayer.EntityFramework.Extensions
{
    public static class ContextProviderExtensions
    {
        public static DbSet<TEntity> Set<TEntity>(this IContextProvider contextProvider)
            where TEntity : class
        {
            return contextProvider.GetCurrentContext()
                .Set<TEntity>();
        }

        public static ObjectContext ObjectContext(this IContextProvider contextProvider)
        {
            return ((IObjectContextAdapter)contextProvider.GetCurrentContext()).ObjectContext;
        }

        public static ObjectSet<TEntity> MakeObjectSet<TEntity>(this IContextProvider contextProvider)
            where TEntity : class
        {
            return contextProvider.ObjectContext()
                .CreateObjectSet<TEntity>();
        }
    }
}

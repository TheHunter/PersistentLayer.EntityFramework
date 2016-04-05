using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PersistentLayer.EntityFramework.Extensions;
using PersistentLayer.Impl;

namespace PersistentLayer.EntityFramework.Impl
{
    public static class EfQueryImplementor
    {
        internal static TEntity FindBy<TEntity>(this IContextProvider contextProvider, object identifier) where TEntity : class
        {
            var set = contextProvider.Set<TEntity>();

            dynamic id = identifier;
            if (identifier.IsCollection())
            {
                id = Enumerable.ToArray(Enumerable.Cast<object>(id));
            }

            return set.Find(id);
        }

        internal static bool Exists<TEntity>(this IContextProvider contextProvider, object identifier) where TEntity : class
        {
            return contextProvider.FindBy<TEntity>(identifier) != null;
        }

        internal static bool Exists<TEntity>(this IContextProvider contextProvider, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return contextProvider.Set<TEntity>().Any(predicate);
        }

        internal static TEntity UniqueResult<TEntity>(this IContextProvider contextProvider, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return contextProvider.Set<TEntity>().SingleOrDefault(predicate);
        }

        internal static IEnumerable<TEntity> FindAll<TEntity>(this IContextProvider contextProvider) where TEntity : class
        {
            return contextProvider.Set<TEntity>().ToList();
        }

        internal static IEnumerable<TEntity> FindAll<TEntity>(this IContextProvider contextProvider, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return contextProvider.Set<TEntity>().Where(predicate)
                .ToList();
        }

        public static IEnumerable<TEntity> FindAllFuture<TEntity>(this IAdvancedContextProvider contextProvider, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var objQuery = contextProvider.ContextProvider.MakeObjectSet<TEntity>().Where(predicate);
            var future = contextProvider.AppendFutureEnumerable(objQuery);

            return future;
        }

        internal static TResult ExecuteExpression<TEntity, TResult>(this IContextProvider contextProvider, Expression<Func<IQueryable<TEntity>, TResult>> queryExpr)
            where TEntity : class
        {
            return queryExpr.Compile().Invoke(contextProvider.Set<TEntity>());
        }

        public static IPagedResult<TEntity> GetPagedResult<TEntity>(this IContextProvider advancedContextProvider, int startIndex, int pageSize,
            Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var counter = advancedContextProvider.Set<TEntity>().Count(predicate);
            var isntances = advancedContextProvider.Set<TEntity>().Skip(startIndex)
                .Take(pageSize)
                .Where(predicate);

            return new PagedResult<TEntity>(startIndex, pageSize, counter, isntances.ToList());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using PersistentLayer.EntityFramework.Extensions;
using PersistentLayer.EntityFramework.Future;
using PersistentLayer.EntityFramework.Reflection;

namespace PersistentLayer.EntityFramework.Impl
{
    public class AdvancedContextProvider
        : IAdvancedContextProvider
    {
        private readonly List<IFutureQuery> futureQueries;

        public AdvancedContextProvider(IContextProvider contextProvider)
        {
            this.futureQueries = new List<IFutureQuery>();
            this.ContextProvider = contextProvider;
        }

        public IContextProvider ContextProvider { get; private set; }

        public IEnumerable<TEntity> AppendFutureEnumerable<TEntity>(IQueryable<TEntity> query)
        {
            var future = new FutureQueryEnumerable<TEntity>(query, this.ExecuteBatch);
            this.futureQueries.Add(future);

            return future;
        }

        public void ExecuteBatch()
        {
            // execute all future queries..

            var context = this.ContextProvider.ObjectContext();

            if (context == null)
                throw new ArgumentNullException("context");
            if (futureQueries == null)
                throw new ArgumentNullException("futureQueries");

            // used to call internal methods
            dynamic contextProxy = new DynamicProxy(context);
            contextProxy.EnsureConnection(false);

            //the (internal) InterceptionContext contains the registered loggers
            DbInterceptionContext interceptionContext = contextProxy.InterceptionContext;

            try
            {
                using (var command = context.CreateFutureCommand(futureQueries))
                using (var reader = DbInterception.Dispatch.Command.Reader(command, new DbCommandInterceptionContext(interceptionContext)))
                {
                    foreach (var futureQuery in futureQueries)
                    {
                        futureQuery.SetResult(context, reader);
                        reader.NextResult();
                    }
                }
            }
            finally
            {
                contextProxy.ReleaseConnection();
                // once all queries processed, clear from queue
                this.futureQueries.Clear();
            }
        }
    }
}

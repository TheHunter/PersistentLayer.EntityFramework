using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Linq;
using PersistentLayer.EntityFramework.Reflection;

namespace PersistentLayer.EntityFramework.Future
{
    public abstract class FutureQuery<TEntity> : IFutureQuery
    {
        private readonly IQueryable query;
        private IEnumerable<TEntity> result;
        //private bool isLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="FutureQueryEnumerable{T}"/> class.
        /// </summary>
        /// <param name="query">The query source to use when materializing.</param>
        /// <param name="loadAction">The action to execute when the query is accessed.</param>
        protected FutureQuery(IQueryable query, Action loadAction)
        {
            this.query = query;
            this.LoadAction = loadAction;
            result = null;
        }

        /// <summary>
        /// Gets the action to execute when the query is accessed.
        /// </summary>
        /// <value>The load action.</value>
        protected Action LoadAction { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is loaded.
        /// </summary>
        /// <value><c>true</c> if this instance is loaded; otherwise, <c>false</c>.</value>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Gets or sets the query execute exception. 
        /// </summary>
        /// <value>The query execute exception.</value>      
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets the query source to use when materializing.
        /// </summary>
        /// <value>The query source to use when materializing.</value>
        IQueryable IFutureQuery.Query
        {
            get { return query; }
        }

        /// <summary>
        /// Gets the result by invoking the <see cref="LoadAction"/> if not already loaded.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> that can be used to iterate through the collection.
        /// </returns>
        protected virtual IEnumerable<TEntity> GetResult()
        {
            if (this.IsLoaded)
                return result;

            // no load action, run query directly
            if (this.LoadAction == null)
            {
                this.IsLoaded = true;
                this.result = query as IEnumerable<TEntity>;
                return this.result;
            }

            // invoke the load action on the datacontext
            // result will be set with a callback to SetResult
            this.LoadAction.Invoke();
            return this.result ?? Enumerable.Empty<TEntity>();
        }

        /// <summary>
        /// Gets the data command for this query.
        /// </summary>
        /// <param name="dataContext">The data context to get the command from.</param>
        /// <returns>The requested command object.</returns>
        QueryPlan IFutureQuery.GetPlan(ObjectContext dataContext)
        {
            return GetPlan(dataContext);
        }

        /// <summary>
        /// Gets the data command for this query.
        /// </summary>
        /// <param name="dataContext">The data context to get the command from.</param>
        /// <returns>The requested command object.</returns>
        protected virtual QueryPlan GetPlan(ObjectContext dataContext)
        {
            IFutureQuery futureQuery = this;
            var source = futureQuery.Query;

            var q = source as ObjectQuery;
            if (q == null)
                throw new InvalidOperationException("The future query is not of type ObjectQuery.");

            var plan = new QueryPlan
            {
                CommandText = q.ToTraceString(),
                Parameters = q.Parameters
            };

            return plan;
        }

        /// <summary>
        /// Sets the underling value after the query has been executed.
        /// </summary>
        /// <param name="dataContext">The data context to translate the results with.</param>
        /// <param name="reader">The <see cref="DbDataReader"/> to get the result from.</param>
        void IFutureQuery.SetResult(ObjectContext dataContext, DbDataReader reader)
        {
            SetResult(dataContext, reader);
        }

        /// <summary>
        /// Sets the underling value after the query has been executed.
        /// </summary>
        /// <param name="dataContext">The data context to translate the results with.</param>
        /// <param name="reader">The <see cref="DbDataReader"/> to get the result from.</param>
        protected virtual void SetResult(ObjectContext dataContext, DbDataReader reader)
        {
            this.IsLoaded = true;

            try
            {
                IFutureQuery futureQuery = this;
                var source = futureQuery.Query;

                var q = source as ObjectQuery;
                if (q == null)
                    throw new InvalidOperationException("The future query is not of type ObjectQuery.");

                // create execution plan
                dynamic queryProxy = new DynamicProxy(q);
                // ObjectQueryState
                dynamic queryState = queryProxy.QueryState;
                // ObjectQueryExecutionPlan
                dynamic executionPlan = queryState.GetExecutionPlan(null);

                // ShaperFactory
                dynamic shaperFactory = executionPlan.ResultShaperFactory;
                // Shaper<T>
                dynamic shaper = shaperFactory.Create(reader, dataContext, dataContext.MetadataWorkspace, MergeOption.AppendOnly, false, true, false);

                var list = new List<TEntity>();
                IEnumerator<TEntity> enumerator = shaper.GetEnumerator();
                while (enumerator.MoveNext())
                    list.Add(enumerator.Current);

                result = list;

                // translate has issue with column names not matching
                //var resultSet = dataContext.Translate<T>(reader);
                //result = resultSet.ToList();
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
        }
    }
}

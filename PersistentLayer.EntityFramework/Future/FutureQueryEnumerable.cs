using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PersistentLayer.EntityFramework.Future
{
    /// <summary>
    /// Provides for defering the execution to a batch of queries.
    /// </summary>
    /// <typeparam name="TEntity">The type for the future query.</typeparam>
    /// <example>The following is an example of how to use FutureQuery.
    /// <code><![CDATA[
    /// var db = new TrackerDataContext { Log = Console.Out };
    /// // build up queries
    /// var q1 = db.User.ByEmailAddress("one@test.com").Future();
    /// var q2 = db.Task.Where(t => t.Summary == "Test").Future();
    /// // this triggers the loading of all the future queries as a batch
    /// var users = q1.ToList();
    /// var tasks = q2.ToList();
    /// ]]>
    /// </code>
    /// </example>    
    [DebuggerDisplay("IsLoaded={IsLoaded}")]
    public class FutureQueryEnumerable<TEntity> : FutureQuery<TEntity>, IEnumerable<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:EntityFramework.Future.FutureQuery`1"/> class.
        /// </summary>
        /// <param name="query">The query source to use when materializing.</param>
        /// <param name="loadAction">The action to execute when the query is accessed.</param>
        internal FutureQueryEnumerable(IQueryable query, Action loadAction)
            : base(query, loadAction)
        {
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TEntity> GetEnumerator()
        {
            // triggers loading future queries  
            var result = this.GetResult() ?? Enumerable.Empty<TEntity>();

            if (Exception != null)
                throw new FutureException("An error occurred executing the future query.", Exception);

            return result.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

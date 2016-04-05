using System;
using System.Diagnostics;
using System.Linq;

namespace PersistentLayer.EntityFramework.Future
{
    public class FutureValue<TEntity> : FutureQuery<TEntity>, IFutureValue<TEntity>
    {
        private bool hasValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EntityFramework.Future.FutureValue`1"/> class.
        /// </summary>
        /// <param name="query">The query source to use when materializing.</param>
        /// <param name="loadAction">The action to execute when the query is accessed.</param>
        internal FutureValue(IQueryable query, Action loadAction)
            : base(query, loadAction)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EntityFramework.Future.FutureValue`1"/> class.
        /// </summary>
        /// <param name="underlyingValue">The underlying value.</param>
        public FutureValue(TEntity underlyingValue)
            : base(null, null)
        {
            UnderlyingValue = underlyingValue;
            hasValue = true;
        }

        /// <summary>
        /// Gets or sets the value assigned to or loaded by the query.
        /// </summary>
        /// <returns>
        /// The value of this deferred property.
        /// </returns>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public TEntity Value
        {
            get
            {
                if (!hasValue)
                {
                    hasValue = true;

                    var result = GetResult() ?? Enumerable.Empty<TEntity>();
                    UnderlyingValue = result.FirstOrDefault();
                }

                if (Exception != null)
                    throw new FutureException("An error occurred executing the future query.", Exception);

                return UnderlyingValue;
            }
            set
            {
                UnderlyingValue = value;
                hasValue = true;
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="T:EntityFramework.Future.FutureValue`1" /> to T.
        /// </summary>
        /// <param name="futureValue">The future value.</param>
        /// <returns>
        /// The result of forcing this lazy value.
        /// </returns>
        public static implicit operator TEntity(FutureValue<TEntity> futureValue)
        {
            return futureValue.Value;
        }

        /// <summary>
        /// Gets the underling value. This property will not trigger the loading of the future query.
        /// </summary>
        /// <value>The underling value.</value>
        internal TEntity UnderlyingValue { get; private set; }
    }
}

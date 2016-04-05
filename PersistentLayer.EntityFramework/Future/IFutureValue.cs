namespace PersistentLayer.EntityFramework.Future
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IFutureValue<out TResult>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        TResult Value { get; }
    }
}

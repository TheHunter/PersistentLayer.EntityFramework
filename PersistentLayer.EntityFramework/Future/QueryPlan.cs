using System.Data.Entity.Core.Objects;

namespace PersistentLayer.EntityFramework.Future
{
    public class QueryPlan
    {
        /// <summary>
        /// Gets or sets the command text.
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        public string CommandText { get; set; }
        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public ObjectParameterCollection Parameters { get; set; }
    }
}

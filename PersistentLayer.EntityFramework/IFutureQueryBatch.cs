using System.Collections.Generic;
using System.Linq;

namespace PersistentLayer.EntityFramework
{
    public interface IFutureQueryBatch
    {
        IEnumerable<TEntity> AppendFutureEnumerable<TEntity>(IQueryable<TEntity> query);

        void ExecuteBatch();
    }
}

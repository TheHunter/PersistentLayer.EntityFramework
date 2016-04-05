using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace PersistentLayer.EntityFramework.Test.Extensions
{
    public static class DbContextExtensions
    {
        public static ObjectContext ObjectContext(this DbContext context)
        {
            return ((IObjectContextAdapter)context).ObjectContext;
        }
    }
}

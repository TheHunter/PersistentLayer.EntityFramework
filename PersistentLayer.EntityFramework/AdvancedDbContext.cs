using System;
using System.Data.Entity;

namespace PersistentLayer.EntityFramework
{
    public class AdvancedDbContext : DbContext
    {
        private readonly Action<DbModelBuilder> onModelCreating;

        public AdvancedDbContext(string connectionStr, Action<DbModelBuilder> onModelCreating)
            : base(connectionStr)
        {
            this.onModelCreating = onModelCreating;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AdvancedDbContext>(null);

            base.OnModelCreating(modelBuilder);
            this.onModelCreating.Invoke(modelBuilder);
        }
    }
}

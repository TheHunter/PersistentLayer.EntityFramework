using System.Data.Entity.ModelConfiguration;
using PersistentLayer.EntityFramework.Test.Pocos;

namespace PersistentLayer.EntityFramework.Test.Mappings
{
    public class SalesmanMap : EntityTypeConfiguration<Salesman>
    {
        public SalesmanMap()
        {
            this.ToTable("Salesman");
            this.HasKey(salesman => salesman.Id);

            this.Property(salesman => salesman.Id);
            this.Property(salesman => salesman.Name);
            this.Property(salesman => salesman.Surname);
            this.Property(salesman => salesman.Email);
            this.Property(salesman => salesman.IdentityCode);
        }
    }
}

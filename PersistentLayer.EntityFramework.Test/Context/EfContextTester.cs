using System.Data.Entity;
using System.Linq;
using PersistentLayer.EntityFramework.Test.Extensions;
using PersistentLayer.EntityFramework.Test.Pocos;
using Xunit;

namespace PersistentLayer.EntityFramework.Test.Context
{
    public class EfContextTester
        : ConfigurationTester
    {
        [Fact]
        public void TestOnRead()
        {
            using (var context = this.MakeDbContext())
            {
                var instance = context.Set<Salesman>()
                    .SingleOrDefault(salesman => salesman.Id == 1);

                Assert.NotNull(instance);
            }
        }

        [Fact]
        public void StateInstance()
        {
            using (var context = this.MakeDbContext())
            {
                var instance = new Salesman
                {
                    Id = -15, Name = "maria", Surname = "lopez", Email = "maria.lopez@hotmail.com"
                };

                var entry = context.Entry(instance);
                Assert.NotNull(entry);
                Assert.Equal(EntityState.Detached, entry.State);


                var instance0 = context.Set<Salesman>()
                    .SingleOrDefault(salesman => salesman.Id == 1);

                Assert.NotNull(instance0);

                var entry0 = context.Entry(instance0);
                Assert.NotNull(entry0);
                Assert.NotEqual(EntityState.Detached, entry0.State);

                Assert.Equal(EntityState.Unchanged, entry0.State);

                // modify instance.
                instance0.Email = instance0.Email + "_ciao";
                entry0 = context.Entry(instance0);
                Assert.Equal(EntityState.Modified, entry0.State);

                var objContext = context.ObjectContext();
                var objSet = objContext.CreateObjectSet<Salesman>();
                Assert.NotNull(objSet);

                //objSet.EntitySet.MetadataProperties
                //objSet.EntitySet.ElementType.KeyMembers[0].

                /*
                try to understand if an instance is detached.. 
                this can be understood by entity state

                Another thing to consider is to understand if an instance has set the identity
                property, so in that case It will be applied an update operation, otherwise It wuld be treated as new instance.
                */
            }
        }
    }
}

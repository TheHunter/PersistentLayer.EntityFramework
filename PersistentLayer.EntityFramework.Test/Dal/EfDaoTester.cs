using System.Linq;
using PersistentLayer.EntityFramework.Impl;
using PersistentLayer.EntityFramework.Test.Pocos;
using Xunit;

namespace PersistentLayer.EntityFramework.Test.Dal
{
    public class EfDaoTester
        : ConfigurationTester
    {
        [Fact]
        public void Test1()
        {
            var dao = new EfRootPagedDAO<object>(this.MakeTransactionProvider());

            var instance = dao.UniqueResult<Salesman>(salesman => salesman.Id == 1);
            Assert.NotNull(instance);

        }

        [Fact]
        public void Test2()
        {
            var ctx = this.MakeAdvancedContextProvider();

            var future = ctx.FindAllFuture<Salesman>(salesman => salesman.Id < 10);

            Assert.NotNull(future);
            var count = future.Count();

            Assert.NotEmpty(future);
        }
    }
}

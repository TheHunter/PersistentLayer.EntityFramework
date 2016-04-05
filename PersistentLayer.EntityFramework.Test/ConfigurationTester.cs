using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using PersistentLayer.EntityFramework.Impl;
using PersistentLayer.EntityFramework.Test.Mappings;

namespace PersistentLayer.EntityFramework.Test
{
    public class ConfigurationTester
    {
        string rootPathProject;

        public ConfigurationTester()
        {
            this.SetRootPathProject();
        }

        private void SetRootPathProject()
        {
            var list = new List<string>(Directory.GetCurrentDirectory().Split('\\'));
            list.RemoveAt(list.Count - 1);
            list.RemoveAt(list.Count - 1);
            list.Add(string.Empty);
            this.rootPathProject = string.Join("\\", list.ToArray());
        }

        protected string GetConnectionString()
        {
            string output = this.rootPathProject + "db\\";
            var str = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
            return string.Format(str, output);
            //return str;
        }

        protected DbContext MakeDbContext()
        {
            return new AdvancedDbContext(this.GetConnectionString(), builder =>
            {
                builder.Configurations.Add(new SalesmanMap());
            });
        }

        protected IContextProvider MakeContextProvider()
        {
            return new ContextProvider(this.MakeDbContext());
        }

        protected IEfTransactionProvider MakeTransactionProvider()
        {
            return new EfTransactionProvider(this.MakeContextProvider());
        }

        protected IFutureContextProvider MakeAdvancedContextProvider()
        {
            return new FutureContextProvider(this.MakeContextProvider());
        }
    }
}

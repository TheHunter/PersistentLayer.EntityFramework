using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.EntityFramework.Test.Pocos
{
    public class Salesman
    {
        public long? Id
        {
            get; set;
        }

        public virtual string Name
        {
            get; set;
        }

        public virtual string Surname
        {
            get; set;
        }

        public virtual string Email
        {
            get; set;
        }

        public virtual int? IdentityCode
        {
            get; set;
        }
    }
}

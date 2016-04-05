using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.EntityFramework.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsCollection(this object instance)
        {
            if (instance == null)
                return false;

            var type = instance.GetType();

            return type.GetInterfaces().Any(type1 => type1 == typeof(IEnumerable));
        }

        public static bool IsArray(this object instance)
        {
            if (instance == null)
                return false;

            var type = instance.GetType();

            return type.IsArray;
        }
    }
}

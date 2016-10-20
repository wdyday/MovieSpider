using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Extentions
{
    public static class AssemblyExtension
    {
        public static List<Type> GetInheritedTypes(this Assembly assembly, Type baseType)
        {
            return assembly.GetTypes()
                .Where(x => x.BaseType != null && x.BaseType.GenericEq(baseType))
                .ToList();
        }
    }
}

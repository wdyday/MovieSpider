using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Extentions
{
    public static class ValueInjecterExtension
    {
        public static ICollection<TTo> InjectFrom<TFrom, TTo>(this ICollection<TTo> to, params IEnumerable<TFrom>[] sources) where TTo : new()
        {
            foreach (var from in sources)
            {
                foreach (var source in from)
                {
                    var target = new TTo();
                    target.InjectFrom(source);
                    to.Add(target);
                }
            }
            return to;
        }
    }
}

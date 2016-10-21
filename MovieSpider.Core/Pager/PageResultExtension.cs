using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Pager
{
    public static class PageResultExtension
    {
        public static PageResult<T> ToPageResult<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return new PageResult<T>(source, pageIndex, pageSize, source.Count());
        }

        public static PageResult<T> ToPageResult<T>(this IQueryable<T> linq, int pageIndex, int pageSize)
        {
            return new PageResult<T>(linq, pageIndex, pageSize);
        }

        public static PageResult<T> ToPageResult<T>(this IQueryable<T> query, PageRequest request)
        {
            return new PageResult<T>(query.OrderBy(request.Sort, request.SortDirection), request.PageIndex, request.PageSize);
        }

        public static PageResult<T> ToPageResult<T>(this IList<T> source, int pageIndex, int pageSize)
        {
            return new PageResult<T>(source, pageIndex, pageSize);
        }
    }
}

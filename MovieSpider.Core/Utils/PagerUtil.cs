using MovieSpider.Core.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Utils
{
    public class PagerUtil
    {
        public static int CalculatePageCount(int rowCount)
        {
            var pageCount = rowCount % CommonConst.PageSize == 0 ? rowCount / CommonConst.PageSize : rowCount / CommonConst.PageSize + 1;
            return pageCount;
        }
    }
}

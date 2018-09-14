using MovieSpider.Data.DbEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Data.Models
{
    public class Dy2018Model
    {
        /// <summary>
        /// 当前抓取的list 页号
        /// 第一页一直更新DB
        /// </summary>
        public int PageIndex { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public RegionEnum Country { get; set; }

        public MediaTypeEnum MediaType { get; set; }
    }
}

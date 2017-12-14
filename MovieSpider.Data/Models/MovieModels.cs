using MovieSpider.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Data.Models
{
    public class MovieModel
    {
        public int MovieId { get; set; }

        public string CnName { get; set; }

        /// <summary>
        /// 其他名字: 冰川时代5：星际碰撞/冰原历险记5：笑星撞地球(台)/冰河世纪5/冰川时代5：碰撞航向/冰河世纪：陨石撞地球(港)
        /// </summary>
        public string OtherCnNames { get; set; }

        public string EnName { get; set; }

        public string FromUrl { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        public string Detail { get; set; }

        /// <summary>
        /// 地区: 1-国内, 2-欧美, 3-日韩
        /// </summary>
        public RegionEnum Region { get; set; }

        /// <summary>
        /// 媒体类型
        /// 0: 全部, 1: 电影, 2: 电视剧, 3: 动漫, 4: 综艺
        /// </summary>
        public MediaTypeEnum MediaType { get; set; }

        /// <summary>
        /// 首映日期: 取最小的日期, 如 2016-10-21(中国大陆) / 2016-08-26(美国) / 2016-08-31(法国), 取 2016-08-26(美国)
        /// </summary>
        public DateTime? PremiereDate { get; set; }

        /// <summary>
        /// 首映日期: 2016-10-21(中国大陆) / 2016-08-26(美国) / 2016-08-31(法国)
        /// </summary>
        public string PremiereDateMulti { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 抓取完成
        /// </summary>
        public bool IsDone { get; set; }
    }
}

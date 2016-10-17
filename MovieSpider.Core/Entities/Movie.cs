using MovieSpider.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Entities
{
    public class Movie
    {
        [StringLength(300)]
        public string CnName { get; set; }

        /// <summary>
        /// 其他名字: 冰川时代5：星际碰撞/冰原历险记5：笑星撞地球(台)/冰河世纪5/冰川时代5：碰撞航向/冰河世纪：陨石撞地球(港)
        /// </summary>
        [StringLength(300)]
        public string OtherCnNames { get; set; }

        [StringLength(300)]
        public string EnName { get; set; }

        [StringLength(300)]
        public string FromUrl { get; set; }

        [StringLength(4000)]
        public string Detail { get; set; }

        public CountryEnum Country { get; set; }

        /// <summary>
        /// 首映日期
        /// </summary>
        public DateTime PremiereDate { get; set; }

        /// <summary>
        /// 抓取完成
        /// </summary>
        public bool IsDone { get; set; }
    }
}

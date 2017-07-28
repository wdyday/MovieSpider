using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Data.Enums
{
    public enum RegionEnum
    {
        None = 0,
        China = 1,  //国内
        EuropeOrAmerica,    //欧美
        JapanOrKorea    //日韩
    }

    /// <summary>
    /// 媒体类型
    /// 0: 全部, 1: 电影, 2: 电视剧, 3: 动漫, 4: 综艺
    /// </summary>
    public enum MediaTypeEnum : byte
    {
        None = 0,
        Movie,
        TV,
        Cartoon,
        Variety
    }
}

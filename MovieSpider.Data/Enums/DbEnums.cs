using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Data.DbEnums
{
    public enum GenderEnum
    {
        Unknown = 0,
        Male = 1,
        Famale = 2
    }

    public enum HeadshotEnum
    {
        Big,
        Small
    }

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

    /// <summary>
    /// 同步状态
    /// 1: List抓取完成, 2: Detail抓取完成, 3: 同步web完成
    /// </summary>
    public enum JobStatusEnum : byte
    {
        ListDone = 1,
        DetailDone = 2,
        SyncDone = 3
    }


    public class EnumUtil
    {
        public static string GetRegionEnumName(int region)
        {
            var name = "";
            switch (region)
            {
                case (int)RegionEnum.China: name = "华语"; break;
                case (int)RegionEnum.EuropeOrAmerica: name = "欧美"; break;
                case (int)RegionEnum.JapanOrKorea: name = "日韩"; break;
            }
            return name;
        }
        public static string GetMediaTypeEnumName(int mediaType)
        {
            //    var name = "电影";
            //    switch (mediaType)
            //    {
            //        case (int)MediaTypeEnum.Movie: name = "电影"; break;
            //        case (int)MediaTypeEnum.TV: name = "电视剧"; break;
            //        case (int)MediaTypeEnum.Cartoon: name = "动漫"; break;
            //        case (int)MediaTypeEnum.Variety: name = "综艺"; break;
            //    }

            MediaTypeEnum mediaTypeEnum = (MediaTypeEnum)mediaType;

            return GetMediaTypeEnumName(mediaTypeEnum);
        }

        public static string GetMediaTypeEnumName(MediaTypeEnum? mediaType)
        {
            var name = "";
            if(mediaType != null)
            {
                switch (mediaType)
                {
                    case MediaTypeEnum.Movie: name = "电影"; break;
                    case MediaTypeEnum.TV: name = "电视剧"; break;
                    case MediaTypeEnum.Cartoon: name = "动漫"; break;
                    case MediaTypeEnum.Variety: name = "综艺"; break;
                }
            }
            return name;
        }

        /// <summary>
        /// 地区
        /// </summary>
        public static List<KeyValuePair<RegionEnum, string>> GetRegions()
        {
            return new List<KeyValuePair<RegionEnum, string>>
            {
                new KeyValuePair<RegionEnum, string> (RegionEnum.China, "中国"),
                new KeyValuePair<RegionEnum, string> (RegionEnum.EuropeOrAmerica, "欧美"),
                new KeyValuePair<RegionEnum, string> (RegionEnum.JapanOrKorea, "日韩")
            };
        }

        /// <summary>
        /// 媒体类型
        /// </summary>
        public static List<KeyValuePair<MediaTypeEnum, string>> GetMediaTypes()
        {
            return new List<KeyValuePair<MediaTypeEnum, string>>
            {
                new KeyValuePair<MediaTypeEnum, string> (MediaTypeEnum.Movie, "电影"),
                new KeyValuePair<MediaTypeEnum, string> (MediaTypeEnum.TV, "电视剧"),
                new KeyValuePair<MediaTypeEnum, string> (MediaTypeEnum.Cartoon, "动漫"),
                new KeyValuePair<MediaTypeEnum, string> (MediaTypeEnum.Variety, "综艺")
            };
        }
    }
}

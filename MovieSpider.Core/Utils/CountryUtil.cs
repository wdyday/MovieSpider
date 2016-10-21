using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieSpider.Core.Utils
{
    public class CountryUtil
    {
        public static readonly string[] China = new string[] { "中国", "中国大陆", "香港", "台湾", "澳门" };
        public static readonly string[] EuropeOrAmerica = new string[] { "美国", "法国", "德国", "意大利", "英国", "荷兰", "爱尔兰", "巴西", "乌拉圭" };
        public static readonly string[] JapanOrKorea = new string[] { "日本", "韩国" };

        public static bool IsChina(string country)
        {
            return IsCountry(China, country);
        }
        public static bool IsEuropeOrAmerica(string country)
        {
            return IsCountry(EuropeOrAmerica, country);
        }
        public static bool IsJapanOrKoreaa(string country)
        {
            return IsCountry(JapanOrKorea, country);
        }

        public static bool IsCountry(string[] countries, string country)
        {
            var pattern = string.Format("({0})", string.Join(")|(", countries));
            Regex regex = new Regex(pattern);

            Match match = regex.Match(country);

            return regex.IsMatch(country);
        }
    }
}

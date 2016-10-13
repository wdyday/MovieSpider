﻿using DotnetSpider.Core;
using DotnetSpider.Core.Selector;
using MovieSpider.Enums;
using MovieSpider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieSpider.Utils
{
    public class Dy2018Util
    {
        /// <summary>
        /// 解析html
        /// </summary>
        /// <param name="spiderPage"></param>
        /// <returns></returns>
        public static List<Dy2018Model> ParseHtml(Page spiderPage)
        {
            var items = spiderPage.Selectable.SelectList(Selectors.XPath("//table[@class='tbspan']")).Nodes();

            var models = new List<Dy2018Model>();

            foreach (var item in items)
            {
                // //*[@id="header"]/div/div[3]/div[6]/div[2]/div[2]/div[2]/ul/table[1]/tbody/tr[2]/td[2]/b/a

                bool isSecondATag = item.Select(Selectors.XPath(".//tr[2]/td[2]/b/a[2]")).Nodes().Count() > 0 ? true : false;
                var title = isSecondATag ? item.Select(Selectors.XPath(".//tr[2]/td[2]/b/a[2]")).GetValue() : item.Select(Selectors.XPath(".//tr[2]/td[2]/b/a")).GetValue();
                var url = isSecondATag ? item.Select(Selectors.XPath(".//tr[2]/td[2]/b/a[2]")).Links().GetValue() : item.Select(Selectors.XPath(".//tr[2]/td[2]/b/a")).Links().GetValue().Trim();
                var summary = item.Select(Selectors.XPath(".//tr[4]/td[1]")).GetValue();

                if (url == "http://www.dy2018.com/html/gndy/jddyy/")
                {
                    var a = "";
                }

                var blog = new Dy2018Model
                {
                    Country = GetCountryEnum(summary),
                    Title = title,
                    Url = url
                };

                models.Add(blog);
            }

            return models;
        }

        public static CountryEnum GetCountryEnum(string summary)
        {
            /*
              ◎译    名　星际迷航3：超越星辰/星际迷航13：超越/星际旅行13：超越太空/星空奇遇记13：超域时空(港)/星际争霸战13：浩瀚无垠(台)/星舰奇航记13/星舰迷航记13◎片　　名　Star Trek Beyond◎年　　代　2016◎国　　家　美国◎类　　别　动作/科幻/冒险◎语　　言　英语◎字　　幕　中文字幕◎上映日期　2016-09-02(中国大陆) / 2016-07-22(美国)◎豆瓣评分　7.5/10          
             */

            CountryEnum countryEnum = CountryEnum.China;

            summary = Regex.Replace(summary, @"\s", "");

            Regex regex = new Regex(@"◎国家\w+◎");

            Match match = regex.Match(summary);
            if (match.Success)
            {
                var country = match.Value.Replace("国家", "").Replace("◎", "");

                if (country.Contains("中国"))
                {
                    countryEnum = CountryEnum.China;
                }
                else if (country.Contains("日本"))
                {
                    countryEnum = CountryEnum.JapanAndKorea;
                }
                else if (country.Contains("韩国"))
                {
                    countryEnum = CountryEnum.JapanAndKorea;
                }
                else
                {
                    countryEnum = CountryEnum.EuropeAndAmerica;
                }
            }

            return countryEnum;
        }
    }
}

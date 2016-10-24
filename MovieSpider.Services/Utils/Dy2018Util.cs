using DotnetSpider.Core;
using DotnetSpider.Core.Selector;
using MovieSpider.Core.Consts;
using MovieSpider.Core.Utils;
using MovieSpider.Data.Entities;
using MovieSpider.Data.Enums;
using MovieSpider.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieSpider.Services.Utils
{
    public class Dy2018Util
    {
        #region List
        /// <summary>
        /// 解析html
        /// </summary>
        /// <param name="spiderPage"></param>
        /// <returns></returns>
        public static List<Dy2018Model> ParseListHtml(Page spiderPage)
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

                CountryEnum? countryEnum = GetCountryEnum(summary);
                if (countryEnum == null)
                {
                    countryEnum = GetCountryEnumFromUrl(spiderPage.TargetUrl);
                }
                if (countryEnum == null)
                {
                    countryEnum = CountryEnum.China;
                }

                var blog = new Dy2018Model
                {
                    Country = countryEnum.Value,
                    Title = HtmlUtil.RemoveHTMLTag(title),
                    Url = url
                };

                models.Add(blog);
            }

            return models;
        }

        public static CountryEnum? GetCountryEnum(string summary)
        {
            /*
              ◎译    名　星际迷航3：超越星辰/星际迷航13：超越/星际旅行13：超越太空/星空奇遇记13：超域时空(港)/星际争霸战13：浩瀚无垠(台)/星舰奇航记13/星舰迷航记13◎片　　名　Star Trek Beyond◎年　　代　2016◎国　　家　美国◎类　　别　动作/科幻/冒险◎语　　言　英语◎字　　幕　中文字幕◎上映日期　2016-09-02(中国大陆) / 2016-07-22(美国)◎豆瓣评分　7.5/10          
              ◎译　　名　机械师2：复活/极速秒杀2(台)/机械师2/秒速杀机2(港)◎片　　名　Mechanic: Resurrection◎年　　代　2016◎国　　家　法国/美国◎类　　别　动作/犯罪/惊悚◎语　　言　英语/保加利亚语◎字　　幕　中文字幕◎上映日期　2016-10-21(中国大陆) / 2016-08-26(美国) / 2016-08-31(法国)◎豆瓣评分　6.0/10 from 636 users◎IMDb评分　5.9/10 from 9794 users◎文件格
             */

            CountryEnum? countryEnum = null;

            summary = Regex.Replace(summary, @"\s", "");

            Regex regex = new Regex(@"◎国家([\w+/*]+)◎");

            Match match = regex.Match(summary);
            if (match.Success)
            {
                var country = match.Value.Replace("国家", "").Replace("◎", "");

                var isChina = CountryUtil.IsChina(country);

                if (CountryUtil.IsChina(country))
                {
                    countryEnum = CountryEnum.China;
                }
                else if (CountryUtil.IsJapanOrKoreaa(country))
                {
                    countryEnum = CountryEnum.JapanOrKorea;
                }
                else if (CountryUtil.IsEuropeOrAmerica(country))
                {
                    countryEnum = CountryEnum.EuropeOrAmerica;
                }
            }

            return countryEnum;
        }

        /*
         * 最新 http://www.dy2018.com/html/gndy/dyzz/index.html
         * 国内 http://www.dy2018.com/html/gndy/china/index.html
         * 欧美 http://www.dy2018.com/html/gndy/oumei/index.html
         * 日韩 http://www.dy2018.com/html/gndy/rihan/index.html                
         */
        public static CountryEnum? GetCountryEnumFromUrl(string targetUrl)
        {
            CountryEnum? countryEnum = CountryEnum.China;

            targetUrl = targetUrl.ToLower();
            Regex regex = new Regex(@"http://" + AppSetting.Dy2018Domain + @"/html/gndy/(\w+)/index(_\d+)*\.html");

            var match = regex.Match(targetUrl);

            if (match.Success)
            {
                switch (match.Groups[1].Value)
                {
                    case "china":
                        countryEnum = CountryEnum.China;
                        break;
                    case "oumei":
                        countryEnum = CountryEnum.EuropeOrAmerica;
                        break;
                    case "rihan":
                        countryEnum = CountryEnum.JapanOrKorea;
                        break;
                    case "dyzz":
                        countryEnum = null;
                        break;
                }
            }

            return countryEnum;
        }

        #endregion

        #region Detail

        /// <summary>
        /// 解析详情页html
        /// </summary>
        public static Movie ParseDetailHtml(Page spiderPage)
        {
            var movie = (Movie)spiderPage.Request.GetExtra(spiderPage.TargetUrl);

            // 发布时间
            // xpath: //div[@class='position']/span[@class='updatetime'] 
            var createDateStr = spiderPage.Selectable.Select(Selectors.XPath("//div[@class='position']/span[@class='updatetime']")).GetValue().Trim(); // 发布时间：2016-06-13
            var createDate = GetDate(createDateStr);

            // //*[@id="Zoom"]
            var detailNode = spiderPage.Selectable.Select(Selectors.XPath("//*[@id=\"Zoom\"]"));

            /*
                <p>◎译　　名　机械师2：复活/极速秒杀2(台)/机械师2/秒速杀机2(港)</p>
                <p>◎上映日期　2016-10-21(中国大陆) / 2016-08-26(美国) / 2016-08-31(法国)</p>
                <p>◎片　　名　Mechanic: Resurrection</p>
             */
            var tagPs = detailNode.SelectList(Selectors.XPath(".//p")).Nodes();

            movie.CreateDate = createDate.HasValue ? createDate.Value : movie.CreateDate;
            movie.Detail = detailNode.GetValue();
            movie.OtherCnNames = GetNodeVal(tagPs, "◎译名");
            movie.PremiereDateMulti = GetNodeVal(tagPs, "◎上映日期");
            movie.PremiereDate = GetPremiereDate(movie.PremiereDateMulti);

            return movie;
        }

        private static string GetNodeVal(IList<ISelectable> nodes, string name)
        {
            name = name.Trim();
            var node = nodes.Where(n => n.GetValue().Trim().Contains(name)).FirstOrDefault();
            if (node != null)
            {
                var nodeVal = node.GetValue().Trim();
                var removeHtmlNames = HtmlUtil.RemoveHTMLTag(nodeVal);
                return removeHtmlNames.Trim().Replace(name, "");
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 2016-10-21(中国大陆) / 2016-08-26(美国) / 2016-08-31(法国), 取最小的日期: 2016-08-26(美国)
        /// </summary>
        public static DateTime? GetPremiereDate(string premiereDateMulti)
        {
            if (!string.IsNullOrEmpty(premiereDateMulti))
            {
                Regex regex = new Regex(@"((\d{4})(-|/)(\d{1,2})(-|/)(\d{1,2}))");
                MatchCollection matches = regex.Matches(premiereDateMulti);
                if (matches.Count > 0)
                {
                    List<DateTime> dates = new List<DateTime>();
                    for (var i = 0; i < matches.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(matches[i].Value))
                        {
                            var date = Convert.ToDateTime(matches[i].Value);
                            dates.Add(date);
                        }
                    }

                    if (dates.Count > 0)
                    {
                        return dates.Min(d => d);
                    }
                }
            }

            return null;
        }

        public static DateTime? GetDate(string dateStr)
        {
            Regex regex = new Regex(@"((\d{4})(-|/)(\d{1,2})(-|/)(\d{1,2}))");
            var match = regex.Match(dateStr);
            if (match.Success)
            {
                return Convert.ToDateTime(match.Value);
            }

            return null;
        }

        #endregion
    }
}

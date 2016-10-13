using DotnetSpider.Core;
using DotnetSpider.Core.Selector;
using MovieSpider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                var title = item.Select(Selectors.XPath(".//tbody/tr[2]/td[2]/b/a::text()")).GetValue();
                var url = item.Select(Selectors.XPath(".//tbody/tr[2]/td[2]/b//a")).Links().GetValue().Trim();

                var blog = new Dy2018Model
                {
                    Title = title,
                    Url = url
                };

                models.Add(blog);
            }

            return models;
        }
    }
}

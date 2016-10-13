using MovieSpider.Consts;
using MovieSpider.Spiders;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Jobs
{
    public class Dy2018Job : IJob
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("SwitchBillJob Start! " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            /*
             * 最新 http://www.dy2018.com/html/gndy/dyzz/index.html
             * 国内 http://www.dy2018.com/html/gndy/china/index.html
             * 欧美 http://www.dy2018.com/html/gndy/oumei/index.html
             * 日韩 http://www.dy2018.com/html/gndy/rihan/index.html                
             */

            var siteInfos = new List<SiteInfo>
            {
                new SiteInfo
                {
                    Path = "html/gndy/dyzz",
                    MaxPageNo = 10
                },
                new SiteInfo
                {
                    Path = "html/gndy/china",
                    MaxPageNo = 10
                },
                new SiteInfo
                {
                    Path = "html/gndy/oumei",
                    MaxPageNo = 10
                },
                new SiteInfo
                {
                    Path = "html/gndy/rihan",
                    MaxPageNo = 10
                }
            };

            try
            {
                foreach (var site in siteInfos)
                {
                    var urls = new List<string>();

                    for (var i = 1; i <= site.MaxPageNo; i++)
                    {
                        var index = i == 1 ? "index" : "index_" + i;
                        var url = string.Format("http://{0}/{1}/{2}.html", AppSetting.Dy2018Domain, site.Path, index, i);
                        urls.Add(url);
                    }

                    Dy2018Spider.Run(urls);
                }
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }

            Console.WriteLine("SwitchBillJob End! " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        }
    }
}

public class SiteInfo
{
    public string Path;
    public int MaxPageNo;
}

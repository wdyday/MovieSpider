using MovieSpider.Core.Consts;
using MovieSpider.Core.Ioc;
using MovieSpider.Core.Utils;
using MovieSpider.JobManager.Spiders;
using MovieSpider.Services;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.JobManager.Jobs
{
    /*
     *   DisallowConcurrentExecution
         禁止并发执行多个相同定义的JobDetail, 
         这个注解是加在Job类上的, 但意思并不是不能同时执行多个Job, 而是不能并发执行同一个Job Definition(由JobDetail定义), 但是可以同时执行多个不同的JobDetail
    */
    [DisallowConcurrentExecution]
    public class Dy2018Job : IJob
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public void Execute(IJobExecutionContext context)
        {
            //Console.WriteLine("Dy2018Job Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("Dy2018Job Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));

            /*
             * 最新 http://www.dy2018.com/html/gndy/dyzz/index.html
             * 国内 http://www.dy2018.com/html/gndy/china/index.html
             * 欧美 http://www.dy2018.com/html/gndy/oumei/index.html
             * 日韩 http://www.dy2018.com/html/gndy/rihan/index.html                
             */

            try
            {
                var siteInfos = JsonUtil.GetJsonData<List<SiteInfo>>(Environment.CurrentDirectory + @"\Configs\SiteInfo.json");

                foreach (var site in siteInfos)
                {
                    var urls = new List<string>();

                    if (site.MaxPageNo > 0)
                    {
                        // [大于 0] 循环 1 - MaxPageNo 抓取
                        //for (var i = 1; i <= site.MaxPageNo; i++)
                        //{
                        //    var index = i == 1 ? "index" : "index_" + i;
                        //    var url = string.Format("http://{0}/{1}/{2}.html", AppSetting.Dy2018Domain, site.Path, index, i);
                        //    urls.Add(url);
                        //}
                        for (var i = site.MaxPageNo; i >= 1; i--)
                        {
                            var index = i == 1 ? "index" : "index_" + i;
                            var url = string.Format("http://{0}/{1}/{2}.html", AppSetting.Dy2018Domain, site.Path, index, i);
                            urls.Add(url);
                        }
                    }
                    else if (site.MaxPageNo < 0)
                    {
                        // [小于 0] 抓取当前Url
                        var url = string.Format("http://{0}/{1}", AppSetting.Dy2018Domain, site.Path);
                        urls.Add(url);
                    }

                    if(urls.Count > 0)
                    {
                        Dy2018Spider.Run(urls);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }

            //Console.WriteLine("Dy2018Job End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("Dy2018Job End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
        }
    }
}

public class SiteInfo
{
    public string Path;

    /// <summary>
    /// 1: [大于 0] 循环 1 - MaxPageNo 抓取
    /// 2: [等于 0] 不抓取
    /// 3: [小于 0] 抓取当前Url
    /// </summary>
    public int MaxPageNo; 
}

using DotnetSpider.Core;
using DotnetSpider.Core.Monitor;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Core.Selector;
using MovieSpider.Core.Consts;
using MovieSpider.Core.Ioc;
using MovieSpider.Data.Entities;
using MovieSpider.Data.Models;
using MovieSpider.Services;
using MovieSpider.Services.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace MovieSpider.JobManager.Spiders
{
    public class Dy2018DetailSpider
    {
        public static void Run(List<Movie> movies)
        {
            try
            {
                // 注入监控服务
                //IocContainer.Default.AddSingleton<IMonitor, NLogMonitor>();

                // 定义要采集的 Site 对象, 可以设置 Header、Cookie、代理等
                var site = new Site { EncodingName = AppSetting.Dy2018Encode };

                foreach (var movie in movies)
                {
                    site.AddStartUrl(movie.FromUrl, new Dictionary<string, object> { { movie.FromUrl, movie } });
                }

                // 使用内存Scheduler、自定义PageProcessor、自定义Pipeline创建爬虫
                Spider spider = Spider.Create(site, new Dy2018DetailProcessor(), new QueueDuplicateRemovedScheduler()).AddPipeline(new Dy2018DetailPipeline()).SetThreadNum(1);
                spider.EmptySleepTime = 10 * 1000;
                // 注册爬虫到监控服务
                MonitorCenter.Register(spider);

                // 启动爬虫
                spider.Run();

                //Console.Read();
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Info(ex);
            }
        }

        private class Dy2018DetailProcessor : IPageProcessor
        {
            public Site Site { get; set; }

            public void Process(Page page)
            {
                try
                {
                    var movie = Dy2018Util.ParseDetailHtml(page);

                    if(movie != null)
                    {
                        // 以自定义KEY存入page对象中供Pipeline调用
                        page.AddResultItem(CommonConst.SpiderDetailResult, movie);
                    }
                }
                catch(Exception ex)
                {
                    LogManager.GetCurrentClassLogger().Info(ex);
                }
            }
        }

        private class Dy2018DetailPipeline : BasePipeline
        {
            public override void Process(ResultItems resultItems)
            {
                try
                {
                    var movie = resultItems.Results[CommonConst.SpiderDetailResult] as Movie;

                    if (movie != null)
                    {
                        // 存储到数据库
                        var movieService = Ioc.Get<IMoviceService>();
                        movieService.UpdateMovieDone(movie);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.GetCurrentClassLogger().Info(ex);
                }
            }
        }
    }
}

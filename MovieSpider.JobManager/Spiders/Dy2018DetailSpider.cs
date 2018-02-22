using DotnetSpider.Core;
using DotnetSpider.Core.Downloader;
using DotnetSpider.Core.Monitor;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Core.Selector;
using MovieSpider.Core.Consts;
using MovieSpider.Core.Ioc;
using MovieSpider.Core.Utils;
using MovieSpider.Data.Entities;
using MovieSpider.Data.Models;
using MovieSpider.Services;
using MovieSpider.Services.Utils;
using Newtonsoft.Json;
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
        private NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void Run(List<MovieModel> movies)
        {
            try
            {
                // 注入监控服务
                //IocContainer.Default.AddSingleton<IMonitor, NLogMonitor>();

                // 定义要采集的 Site 对象, 可以设置 Header、Cookie、代理等
                var site = new Site { EncodingName = AppSetting.Dy2018Encode };

                foreach (var movie in movies)
                {
                    site.AddStartUrl(movie.FromUrl, new Dictionary<string, object> { { Dy2018Util.TrimScheme(movie.FromUrl), movie } });
                }

                // 使用内存Scheduler、自定义PageProcessor、自定义Pipeline创建爬虫
                Spider spider = Spider.Create(site, new QueueDuplicateRemovedScheduler(), new Dy2018DetailProcessor()).AddPipeline(new Dy2018DetailPipeline());

                // dowload html by http client
                spider.Downloader = new HttpClientDownloader();
                // 1线程
                spider.ThreadNum = 1;
                // traversal deep 遍历深度
                spider.Deep = 1;

                // 启动爬虫
                spider.Run();

                //Console.Read();
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Info(ex);
            }
        }

        private class Dy2018DetailProcessor : BasePageProcessor
        {
            protected override void Handle(Page page)
            {
                try
                {
                    var movie = Dy2018Util.ParseDetailHtml(page);

                    if (movie != null)
                    {
                        // 以自定义KEY存入page对象中供Pipeline调用
                        page.AddResultItem(CommonConst.SpiderDetailResult, movie);
                    }

                    //LogManager.GetCurrentClassLogger().Info("[内存 Dy2018DetailProcessor End] " + SystemInfo.GetCurrentProcessMemory());
                }
                catch (Exception ex)
                {
                    LogManager.GetCurrentClassLogger().Info(ex);
                }
            }
        }

        private class Dy2018DetailPipeline : BasePipeline
        {
            private NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

            public override void Process(IEnumerable<ResultItems> resultItems)
            {
                //_logger.Info("[内存 Dy2018DetailPipeline Start] " + SystemInfo.GetCurrentProcessMemory());

                MovieModel movie = null;
                try
                {
                    foreach (var resultItem in resultItems)
                    {
                        movie = resultItem.Results[CommonConst.SpiderDetailResult] as MovieModel;

                        if (movie != null)
                        {
                            // 存储到数据库
                            var movieService = Ioc.Get<IMoviceService>();
                            movieService.UpdateMovieDone(movie);
                        }
                    }

                    //_logger.Info("[内存 Dy2018DetailPipeline End] " + SystemInfo.GetCurrentProcessMemory());
                }
                catch (Exception ex)
                {
                    _logger.Info(ex);
                    if (movie != null)
                    {
                        _logger.Info(JsonUtil.JsonToString(movie));
                    }
                }
            }
        }
    }
}

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
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace MovieSpider.JobManager.Spiders
{
    public class Dy2018Spider
    {
        /// <summary>
        /// 启动抓取
        /// </summary>
        public static void Run(List<string> urls)
        {
            try
            {
                // 注入监控服务
                //IocContainer.Default.AddSingleton<IMonitor, NLogMonitor>();

                // 定义要采集的 Site 对象, 可以设置 Header、Cookie、代理等
                var site = new Site { EncodingName = AppSetting.Dy2018Encode };

                site.AddStartUrls(urls);

                // 使用内存Scheduler、自定义PageProcessor、自定义Pipeline创建爬虫
                Spider spider = Spider.Create(site, new QueueDuplicateRemovedScheduler(), new Dy2018Processor()).AddPipeline(new Dy2018Pipeline());

                // dowload html by http client
                spider.Downloader = new HttpClientDownloader();
                // 2线程
                spider.ThreadNum = 2;
                // traversal deep 遍历深度
                spider.Deep = 1;

                // stop crawler if it can't get url from the scheduler after 30000 ms 当爬虫连续30秒无法从调度中心取得需要采集的链接时结束.
                spider.EmptySleepTime = 30000;

                // 启动爬虫
                spider.Run();

                //Console.Read();
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Info(ex);
            }
        }

        private class Dy2018Processor : IPageProcessor
        {
            public Site Site { get; set; }

            public void Process(Page page)
            {
                try
                {
                    var models = Dy2018Util.ParseListHtml(page);

                    // 以自定义KEY存入page对象中供Pipeline调用
                    page.AddResultItem(CommonConst.SpiderResult, models);

                    LogManager.GetCurrentClassLogger().Info("[内存 Dy2018Processor End] " + SystemInfo.GetCurrentProcessMemory());
                }
                catch (Exception ex)
                {
                    LogManager.GetCurrentClassLogger().Info(ex);
                }
            }
        }

        public class Dy2018Pipeline : BasePipeline
        {
            private NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

            public override void Process(IEnumerable<ResultItems> resultItems)
            {
                //_logger.Info("[内存 Dy2018Pipeline Start] " + SystemInfo.GetCurrentProcessMemory());

                var movies = new List<Movie>();

                try
                {
                    foreach (var resultItem in resultItems)
                    {
                        foreach (Dy2018Model model in resultItem.Results[CommonConst.SpiderResult])
                        {
                            //File.AppendAllLines("Dy2018.txt", new[] { model.Title, model.Url, model.Country.ToString() });
                            movies.Add(new Movie
                            {
                                CnName = model.Title,
                                FromUrl = model.Url,
                                Region = model.Country,
                                MediaType = model.MediaType
                            });
                        }
                    }

                    if (movies.Count > 0)
                    {
                        // 可以自由实现插入数据库或保存到文件
                        var movieService = Ioc.Get<IMoviceService>();

                        var fromUrls = movies.Select(m => m.FromUrl).ToList();
                        var dbMovies = movieService.GetMoviesByFromUrls(fromUrls);
                        var dbMovieFromUrls = dbMovies.Select(dbM => dbM.FromUrl).ToList();

                        // 新增
                        var addMovies = movies.Where(m => !dbMovieFromUrls.Contains(m.FromUrl)).ToList();
                        // 更新: 电视剧会更新剧集, 排除电影数据
                        var updateMovies = movies.Where(m => dbMovieFromUrls.Contains(m.FromUrl) && m.MediaType != Data.Enums.MediaTypeEnum.Movie).ToList();

                        if (addMovies.Count > 0)
                        {
                            movieService.AddMovies(addMovies);
                        }
                        if (updateMovies.Count > 0)
                        {
                            movieService.UpdateMovies(updateMovies);
                        }
                    }

                    _logger.Info("[内存 Dy2018Pipeline End] " + SystemInfo.GetCurrentProcessMemory());
                }
                catch (Exception ex)
                {
                    _logger.Info(ex);
                    if (movies.Count > 0)
                    {
                        _logger.Info(JsonUtil.JsonToString(movies));
                    }

                    LogManager.GetCurrentClassLogger().Info(ex);
                }
            }
        }
    }
}

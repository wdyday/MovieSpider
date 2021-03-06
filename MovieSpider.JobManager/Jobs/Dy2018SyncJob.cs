﻿using MovieSpider.Core;
using MovieSpider.Core.Consts;
using MovieSpider.Core.Ioc;
using MovieSpider.Core.Utils;
using MovieSpider.JobManager.Utils;
using MovieSpider.Services;
using Newtonsoft.Json;
using NLog;
using Quartz;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.JobManager.Jobs
{
    /*   抓取同步至网站
     *   DisallowConcurrentExecution
         禁止并发执行多个相同定义的JobDetail, 
         这个注解是加在Job类上的, 但意思并不是不能同时执行多个Job, 而是不能并发执行同一个Job Definition(由JobDetail定义), 但是可以同时执行多个不同的JobDetail
    */
    [DisallowConcurrentExecution]
    public class Dy2018SyncJob : IJob
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _movieDomain = ConfigurationManager.AppSettings["MovieDomain"];

        public void Execute(IJobExecutionContext context)
        {
            //Console.WriteLine("Dy2018SyncJob Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("Dy2018SyncJob Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));

            try
            {
                //_logger.Info("[内存 Dy2018SyncJob Start] " + SystemInfo.GetCurrentProcessMemory());

                //Run();

                SyncToWebDB();

                //_logger.Info("[内存 Dy2018SyncJob End] " + SystemInfo.GetCurrentProcessMemory());
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }

            //Console.WriteLine("Dy2018SyncJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("Dy2018SyncJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
        }

        /// <summary>
        /// 调用web服务器 api 同步数据
        /// web api 可能被空间商屏蔽, 暂不使用
        /// </summary>
        public void Run()
        {
            var restUtils = new RestUtils();

            var movieService = Ioc.Get<IMoviceService>();

            var notSyncCount = movieService.GetNotSyncCount();
            var pageCount = notSyncCount % CommonConst.SyncTopCount == 0 ? notSyncCount / CommonConst.SyncTopCount : notSyncCount / CommonConst.SyncTopCount + 1;

            for (var pageNo = 1; pageNo <= pageCount; pageNo++)
            {
                var movies = movieService.GetNotSyncMovies(pageNo, CommonConst.SyncTopCount);

                if (movies.Count > 0)
                {
                    var result = restUtils.SaveMovies(movies);
                    var movieIds = movies.Select(m => m.MovieId).ToList();
                    if (result.Success)
                    {
                        movieService.UpdateSyncDone(movieIds);
                    }
                    else
                    {
                        _logger.Info(result.Message);
                        _logger.Info($"SyncToWebError, movie ID: {string.Join(",", movieIds)}");
                    }

                    // 休眠 2 秒, 防止调用过快
                    System.Threading.Thread.Sleep(2 * 1000);
                }
            }
        }

        /// <summary>
        /// 直接同步本地库数据到 WEB库
        /// </summary>
        public void SyncToWebDB()
        {
            var postService = new PostService();
            var movieService = Ioc.Get<IMoviceService>();
            var notSyncCount = movieService.GetNotSyncCount();
            var pageCount = notSyncCount % CommonConst.SyncTopCount == 0 ? notSyncCount / CommonConst.SyncTopCount : notSyncCount / CommonConst.SyncTopCount + 1;

            for (var pageNo = 1; pageNo <= pageCount; pageNo++)
            {
                var movies = movieService.GetNotSyncMovies(pageNo, CommonConst.SyncTopCount);

                if (movies.Count > 0)
                {
                    var result = postService.SyncToWebDB(movies);
                    var movieIds = movies.Select(m => m.MovieId).ToList();
                    if (result.Success)
                    {
                        movieService.UpdateSyncDone(movieIds);
                    }
                    else
                    {
                        _logger.Info(result.Message);
                        _logger.Info($"SyncToWebError, movie ID: {string.Join(",", movieIds)}");
                    }

                    // 休眠 2 秒, 防止调用过快
                    System.Threading.Thread.Sleep(2 * 1000);
                }
            }
        }
    }
}

using MovieSpider.Core;
using MovieSpider.Core.Consts;
using MovieSpider.Core.Ioc;
using MovieSpider.Data.Entities;
using MovieSpider.Data.Models;
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
    /*
     *   DisallowConcurrentExecution
         禁止并发执行多个相同定义的JobDetail, 
         这个注解是加在Job类上的, 但意思并不是不能同时执行多个Job, 而是不能并发执行同一个Job Definition(由JobDetail定义), 但是可以同时执行多个不同的JobDetail
    */
    /// <summary>
    /// 网站后台修改的数据同步给 本地抓取库
    /// </summary>
    [DisallowConcurrentExecution]
    public class PostSyncJob : IJob
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _movieDomain = ConfigurationManager.AppSettings["MovieDomain"];

        public void Execute(IJobExecutionContext context)
        {
            //Console.WriteLine("PostSyncJob Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("PostSyncJob Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));

            Run();

            //Console.WriteLine("PostSyncJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("PostSyncJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
        }

        public void Run()
        {
            try
            {
                var restUtils = new RestUtils();
                var movieService = Ioc.Get<IMoviceService>();

                var notSyncCount = restUtils.GetNotSyncPostCount();
                var pageCount = notSyncCount % CommonConst.SyncTopCount == 0 ? notSyncCount / CommonConst.SyncTopCount : notSyncCount / CommonConst.SyncTopCount + 1;

                for (var pageNo = 1; pageNo <= pageCount; pageNo++)
                {
                    var posts = restUtils.GetNotSyncPosts(pageNo, CommonConst.SyncTopCount);

                    if (posts.Count > 0)
                    {
                        // 更新本地库
                        movieService.UpdateMovieByWeb(posts);

                        // 更新 web 端 IsSyncDone
                        var postIds = posts.Select(p => p.PostId).ToList();
                        restUtils.UpdateIsSyncDone(postIds);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }
        }
    }
}

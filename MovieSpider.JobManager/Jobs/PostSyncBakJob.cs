using MovieSpider.Core;
using MovieSpider.Core.Consts;
using MovieSpider.Core.Extentions;
using MovieSpider.Core.Ioc;
using MovieSpider.Data.Entities;
using MovieSpider.Data.IContentEntities;
using MovieSpider.Data.Models;
using MovieSpider.JobManager.Utils;
using MovieSpider.Services;
using MovieSpider.Services.Implementations;
using Newtonsoft.Json;
using NLog;
using Omu.ValueInjecter;
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
    /// 网站数据库备份 同步给新站点
    /// </summary>
    [DisallowConcurrentExecution]
    public class PostSyncBakJob : IJob
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _movieDomain = ConfigurationManager.AppSettings["MovieDomain"];

        public void Execute(IJobExecutionContext context)
        {
            //Console.WriteLine("PostSyncJob Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("PostSyncBakJob Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));

            Run();

            //Console.WriteLine("PostSyncJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("PostSyncBakJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
        }

        public void Run()
        {
            try
            {
                var restUtils = new RestUtils();

                var postService = new PostService();

                var count = postService.GetPostCount();
                var pageCount = count % CommonConst.SyncTopCount == 0 ? count / CommonConst.SyncTopCount : count / CommonConst.SyncTopCount + 1;

                for (var pageNo = 1; pageNo <= pageCount; pageNo++)
                {
                    var posts = postService.GetPostsWithComments(pageNo, CommonConst.SyncTopCount);

                    if (posts.Count > 0)
                    {
                        var result = restUtils.SyncBakPosts(posts);
                        if (!result.Success)
                        {
                            _logger.Info(result.Message);
                        }
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

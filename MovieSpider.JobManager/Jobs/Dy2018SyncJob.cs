using MovieSpider.Core;
using MovieSpider.Core.Consts;
using MovieSpider.Core.Ioc;
using MovieSpider.Core.Utils;
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
                _logger.Info("[内存 Dy2018SyncJob Start] " + SystemInfo.GetCurrentProcessMemory());

                Run();

                _logger.Info("[内存 Dy2018SyncJob End] " + SystemInfo.GetCurrentProcessMemory());
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }

            //Console.WriteLine("Dy2018SyncJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("Dy2018SyncJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
        }

        public void Run()
        {
            var movieService = Ioc.Get<IMoviceService>();

            var notSyncCount = movieService.GetNotSyncCount();
            var pageCount = notSyncCount % CommonConst.SyncTopCount == 0 ? notSyncCount / CommonConst.SyncTopCount : notSyncCount / CommonConst.SyncTopCount + 1;

            for (var pageNo = 1; pageNo <= pageCount; pageNo++)
            {
                var movies = movieService.GetNotSyncMovies(pageNo, CommonConst.SyncTopCount);

                if (movies.Count > 0)
                {
                    RestClient client = new RestClient(_movieDomain);

                    var request = new RestRequest("api/Movie/SaveMovies", Method.POST);
                    request.AddJsonBody(movies);

                    var response = client.Execute(request);

                    var result = JsonConvert.DeserializeObject<ResponseResult>(response.Content);
                    if (result.Success)
                    {
                        var movieIds = movies.Select(m => m.MovieId).ToList();
                        movieService.UpdateSyncDone(movieIds);
                    }
                    else
                    {
                        _logger.Info(result.Message);
                    }

                    // 休眠 10 秒, 防止调用过快
                    System.Threading.Thread.Sleep(10 * 1000);
                }
            }
        }
    }
}

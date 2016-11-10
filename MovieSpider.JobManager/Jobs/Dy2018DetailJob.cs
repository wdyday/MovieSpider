﻿using MovieSpider.Core.Consts;
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
    /// <summary>
    /// 抓取详情页, 每次取100条数据, 每5分钟启动一次, 启动间隔时间不宜太短, 以防重复抓取
    /// </summary>
    public class Dy2018DetailJob : IJob
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Dy2018DetailJob Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("Dy2018DetailJob Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));

            try
            {
                var movieService = Ioc.Get<IMoviceService>();
                var movies = movieService.GetTopNotDoneMovies(CommonConst.TopCount);

                if (movies.Count > 0)
                {
                    Dy2018DetailSpider.Run(movies);
                }
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }

            Console.WriteLine("Dy2018DetailJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("Dy2018DetailJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
        }
    }
}
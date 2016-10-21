using MovieSpider.Core.Consts;
using MovieSpider.Core.Ioc;
using MovieSpider.Core.Utils;
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
                var notDoneCount = movieService.GetNotDoneCount();
                var pageCount = PagerUtil.CalculatePageCount(notDoneCount);

                for (var index = 0; index < pageCount; index++)
                {
                    var movies = movieService.GetMovies(index, CommonConst.PageSize);
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

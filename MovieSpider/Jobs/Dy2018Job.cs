using MovieSpider.Spiders;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Jobs
{
    public class Dy2018Job : IJob
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("SwitchBillJob Start! " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            try
            {
                Dy2018Spider.Run();
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }

            Console.WriteLine("SwitchBillJob End! " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        }
    }
}

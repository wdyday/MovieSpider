using MovieSpider.Core;
using MovieSpider.Core.Consts;
using MovieSpider.Core.Ioc;
using MovieSpider.Core.Utils;
using MovieSpider.Data.Entities;
using MovieSpider.Data.Models;
using MovieSpider.Services;
using Newtonsoft.Json;
using NLog;
using Quartz;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
    /// 引用的第三方包 DotnetSpider 有bug, 会出现内存泄漏, 
    /// 用此Job监测如果内存使用超过 100M 则重启service
    /// </summary>
    [DisallowConcurrentExecution]
    public class RestartJob : IJob
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("RestartJob Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));

            Run();

            _logger.Info("RestartJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
        }

        public void Run()
        {
            try
            {
                var memory = SystemInfo.GetCurrentProcessMemoryMB();
                var maxMemory = int.Parse(ConfigurationManager.AppSettings["RestartMB"]);
                if (memory < maxMemory)
                {
                    return;
                }
                _logger.Info("[RestartJob] Restart! Current Memory : " + memory);

                var proc = new Process();
                proc.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory.EndsWith(@"\")
                    ? AppDomain.CurrentDomain.BaseDirectory + "Bats"
                    : AppDomain.CurrentDomain.BaseDirectory + @"\Bats";
                _logger.Info("[RestartJob] WorkingDirectory: " + proc.StartInfo.WorkingDirectory);
                proc.StartInfo.FileName = @"RestartJob.bat";
                //proc.StartInfo.Arguments = string.Format("10");//this is argument
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }
        }
    }
}

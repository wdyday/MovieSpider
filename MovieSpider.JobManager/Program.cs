using DotnetSpider.Core;
using DotnetSpider.Core.Monitor;
using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.JobManager
{
    /// <summary>
    /// 使用DotnetSpider时,需要引用 HtmlAgilityPack 和 HtmlAgilityPack.CssSelectors, FiddlerCore, Newtonsoft.Json, Microsoft.Extensions.DependencyInjection
    /// </summary>
    class Program
    {
        //static void Main(string[] args)
        //{
        //    NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        //    try
        //    {
        //        // First we must get a reference to a scheduler
        //        NameValueCollection properties = new NameValueCollection();
        //        properties["quartz.scheduler.instanceName"] = "XmlConfiguredInstance";

        //        // set thread pool info
        //        properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
        //        properties["quartz.threadPool.threadCount"] = "10";
        //        properties["quartz.threadPool.threadPriority"] = "Normal";

        //        // job initialization plugin handles our xml reading, without it defaults are used
        //        properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";
        //        properties["quartz.plugin.xml.fileNames"] = "~/quartz_jobs.xml";

        //        ISchedulerFactory sf = new StdSchedulerFactory(properties);
        //        IScheduler sched = sf.GetScheduler();

        //        // start the schedule 
        //        sched.Start();


        //        // 注入爬虫监控服务
        //        IocContainer.Default.AddSingleton<IMonitor, NLogMonitor>();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Info(ex);

        //        Console.WriteLine(ex.Message);
        //        Console.Read();
        //    }
        //}

        static void Main()
        {
            // set the current directory to the same directory as your windows service
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MovieSpiderWindowsService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}

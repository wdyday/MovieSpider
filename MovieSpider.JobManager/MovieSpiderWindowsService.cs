using DotnetSpider.Core;
using DotnetSpider.Core.Infrastructure;
using DotnetSpider.Core.Monitor;
using MovieSpider.Services.IContentCache;
using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.JobManager
{
    /// <summary>
    /// MovieSpider Service
    /// 创建服务:
    ///     sc create MovieSpiderService binPath= "F:\Project\Movie\MovieSpider\MovieSpider.JobManager\bin\Release\MovieSpider.JobManager.exe"  displayname= "MovieSpiderService"
    /// 配置服务:
    ///     sc config MovieSpiderService start = AUTO    (自动) 
    ///     sc config MovieSpiderService start = DEMAND(手动)
    ///     sc config MovieSpiderService start = DISABLED(禁用)
    /// 启动服务:
    ///     net start MovieSpiderService
    /// 关闭服务:
    ///     net stop MovieSpiderService
    /// 删除服务:
    ///     sc delete MovieSpiderService
    /// </summary>
    partial class MovieSpiderWindowsService : ServiceBase
    {
        private NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly string DateFormatYmdhms = "yyyy/MM/dd HH:mm:ss";

        public MovieSpiderWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // First we must get a reference to a scheduler
                NameValueCollection properties = new NameValueCollection();
                properties["quartz.scheduler.instanceName"] = "XmlConfiguredInstance";

                // set thread pool info
                properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
                properties["quartz.threadPool.threadCount"] = "10";
                properties["quartz.threadPool.threadPriority"] = "Normal";

                // job initialization plugin handles our xml reading, without it defaults are used
                properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";
                properties["quartz.plugin.xml.fileNames"] = "~/quartz_jobs.xml";

                ISchedulerFactory sf = new StdSchedulerFactory(properties);
                IScheduler sched = sf.GetScheduler();

                // start the schedule 
                sched.Start();

                // cache
                IContentCacher.Init();
            }
            catch (Exception ex)
            {
                _logger.Info(ex);

                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }
        
        protected override void OnStop()
        {
            _logger.Info("service: OnStop: " + DateTime.Now.ToString(DateFormatYmdhms));
        }

        protected override void OnPause()
        {
            base.OnPause();
            _logger.Info("service: OnPause: " + DateTime.Now.ToString(DateFormatYmdhms));
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();

            _logger.Info("service: OnShutdown: " + DateTime.Now.ToString(DateFormatYmdhms));
        }
    }
}

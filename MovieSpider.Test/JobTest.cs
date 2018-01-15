using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieSpider.Core.Utils;
using MovieSpider.Data.Enums;
using MovieSpider.Services.Utils;
using MovieSpider.Core.Ioc;
using MovieSpider.Services;
using MovieSpider.Core.Consts;
using MovieSpider.JobManager.Jobs;

namespace MovieSpider.Test
{
    [TestClass]
    public class JobTest
    {
        [TestMethod]
        public void PostSyncJobTest()
        {
            new PostSyncJob().Run();
        }

        [TestMethod]
        public void Dy2018DetailJobTest()
        {
            new Dy2018DetailJob().Run();
        }

        [TestMethod]
        public void Dy2018SyncJobTest()
        {
            new Dy2018SyncJob().Run();
        }

        [TestMethod]
        public void RestartJobTest()
        {
            //new RestartJob().Run();
            var _ServiceName = "MovieSpiderService";

            var restartTime = WinServiceUtil.GetRestartTime(_ServiceName);
            var restartedMinutes = DiffMinutes(restartTime, DateTime.Now);
            if (restartedMinutes >= 5)
            {
                WinServiceUtil.Reset(_ServiceName);
            }
            if (WinServiceUtil.IsRestarted(_ServiceName))
            {
                //_logger.Info("[RestartJob] IsRestarted ...");
                return;
            }

            var batFileName = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')}\\Bats\\RestartJob.bat";
            WinServiceUtil.Restart(_ServiceName, batFileName);
        }
        private int DiffMinutes(DateTime dateFrom, DateTime dateTo)
        {
            return (dateTo - dateFrom).Minutes;
        }
    }
}

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
    }
}

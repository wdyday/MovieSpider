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
    }
}

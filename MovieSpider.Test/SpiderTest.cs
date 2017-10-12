using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieSpider.Core.Utils;
using MovieSpider.Data.Enums;
using MovieSpider.Services.Utils;
using MovieSpider.Core.Ioc;
using MovieSpider.Services;
using MovieSpider.Core.Consts;
using MovieSpider.JobManager.Jobs;
using MovieSpider.JobManager.Spiders;
using System.Collections.Generic;
using MovieSpider.Data.Entities;

namespace MovieSpider.Test
{
    [TestClass]
    public class SpiderTest
    {
        [TestMethod]
        public void Dy2018DetailSpiderTest()
        {
            // http://www.dy2018.com/i/92382.html
            var movie = new MoviceService().Get(32752);
            var movies = new List<Movie>
            {
                movie
            };
            Dy2018DetailSpider.Run(movies);
        }
    }
}

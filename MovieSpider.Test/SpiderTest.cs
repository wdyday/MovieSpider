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
using MovieSpider.Data.Models;

namespace MovieSpider.Test
{
    [TestClass]
    public class SpiderTest
    {
        [TestMethod]
        public void Dy2018SpiderTest()
        {
            var urls = new List<string>
            {
                "http://www.dy2018.com/html/gndy/dyzz/index.html"
            };
            Dy2018Spider.Run(urls);
        }

        [TestMethod]
        public void Dy2018DetailSpiderTest()
        {
            // http://www.dy2018.com/i/98780.html
            var movie = new MoviceService().Get(26939);
            var movies = new List<MovieModel>
            {
                new MovieModel
                {
                    MovieId = movie.MovieId,
                    FromUrl = movie.FromUrl,
                    CreateTime = movie.CreateTime
                }
            };
            Dy2018DetailSpider.Run(movies);
        }

        [TestMethod]
        public void CountryUtilTest()
        {
            var title = "2017年美国7.7分恐怖片《小丑回魂IT》HD韩版中字";

            var isEuropeOrAmerica = CountryUtil.IsEuropeOrAmerica(title);

            Assert.IsTrue(isEuropeOrAmerica);

            var isChina = CountryUtil.IsChina(title);
            Assert.IsFalse(isChina);
        }
    }
}

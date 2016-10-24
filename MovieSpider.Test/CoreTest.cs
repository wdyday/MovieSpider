using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieSpider.Core.Utils;
using MovieSpider.Data.Enums;
using MovieSpider.Services.Utils;
using MovieSpider.Core.Ioc;
using MovieSpider.Services;
using MovieSpider.Core.Consts;

namespace MovieSpider.Test
{
    [TestClass]
    public class CoreTest
    {
        [TestMethod]
        public void CountryUtilTest()
        {
            var isChina = CountryUtil.IsChina("中国大陆/香港");

            Assert.IsTrue(isChina);
        }

        [TestMethod]
        public void Dy2018UtilTest_GetCountryEnum()
        {
            var summary = "◎译　　名　猩红山峰/血色庄园(港)/腥红山庄(台)◎片　　名　Crimson Peak◎年　　代　2015◎国　　家　美国/加拿大◎类　　别　剧情/悬疑/奇幻/惊悚◎语　　言　英语◎字　　幕　中英双字幕◎";
            CountryEnum? countryEnum = Dy2018Util.GetCountryEnum(summary);

            Assert.IsTrue(countryEnum.Value == CountryEnum.EuropeOrAmerica);
        }

        [TestMethod]
        public void Dy2018UtilTest_GetPremiereDate()
        {
            var date = Dy2018Util.GetPremiereDate("2016-10-21(中国大陆) / 2016-08-26(美国) / 2016-08-31(法国)");

            Assert.IsTrue(date.HasValue);
        }

        [TestMethod]
        public void Dy2018UtilTest_GetMovies()
        {
            var movieService = Ioc.Get<IMoviceService>();
            var movies = movieService.GetTopNotDoneMovies(CommonConst.PageSize);
        }
    }
}

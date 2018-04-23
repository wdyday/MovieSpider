using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieSpider.Core.Utils;
using MovieSpider.Data.DbEnums;
using MovieSpider.Services.Utils;
using MovieSpider.Core.Ioc;
using MovieSpider.Services;
using MovieSpider.Core.Consts;
using MovieSpider.Core.Crypto;
using System.Collections.Generic;
using System.Web;

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
            var summary = "◎译　　名　猩红山峰/血色庄园(港)/腥红山庄(台)◎片　　名　Crimson Peak◎年　　代　2015◎地　　区　美国◎类　　别　剧情/悬疑/奇幻/惊悚◎语　　言　英语◎字　　幕　中英双字幕◎";
            RegionEnum? countryEnum = Dy2018Util.GetCountryEnum(summary);

            Assert.IsTrue(countryEnum.Value == RegionEnum.EuropeOrAmerica);
        }

        [TestMethod]
        public void Dy2018UtilTest_GetCountryEnumFromUrl()
        {
            var url = "http://www.dy2018.com/html/tv/oumeitv/index.html";
            RegionEnum? countryEnum = Dy2018Util.GetCountryEnumFromUrl(url);

            Assert.IsTrue(countryEnum.Value == RegionEnum.EuropeOrAmerica);
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
            //var movies = movieService.GetTopNotDoneMovies(CommonConst.PageSize);
        }

        [TestMethod]
        public void MovieServiceTest_GetNotDoneMovies()
        {
            var movieService = Ioc.Get<IMoviceService>();
            var m1 = SystemInfo.GetCurrentProcessMemory();
            var movies = movieService.GetListDoneMovies(1, CommonConst.TopCount);
            var m2 = SystemInfo.GetCurrentProcessMemory();

            Assert.IsTrue(m2 != m1);
        }

        [TestMethod]
        public void CryptoUtilsTest_ComputeHash()
        {
            var key = "531ce320a4a341a691b37666a92d3e84";
            var token = "f93ff0180c0d417e8e899cc6eda21666";
            var val0 = CryptoUtils.ComputeHash(key + token);
            var val0Encode = HttpUtility.UrlEncode(val0);
            var val1 = CryptoUtils.ComputeHash("123456");
            var val2 = HttpUtility.UrlEncode(CryptoUtils.ComputeHash("123456"));
        }

        [TestMethod]
        public void CryptoUtilsTest_Encrypt()
        {
            var key = CryptoUtils.Encrypt("123456");
        }

        [TestMethod]
        public void FileUtilTest_GetJson()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\ServiceLogs\\Log.Json";
            var info = FileUtil.GetJson<RestartInfo>(path);
        }

        [TestMethod]
        public void FileUtilTest_Save()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\ServiceLogs\\Log.Json";
            var info = new List<RestartInfo>
            {
                new RestartInfo()
                {
                    ServiceName = "ServiceName",
                    IsRestarted = false,
                    RestartTime = DateTime.MaxValue
                }
            };
            FileUtil.Save(path, info);
        }
    }
}

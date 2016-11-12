using DotnetSpider.Core;
using DotnetSpider.Core.Selector;
using MovieSpider.Core.Consts;
using MovieSpider.Core.Utils;
using MovieSpider.Data.Entities;
using MovieSpider.Data.Enums;
using MovieSpider.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieSpider.Services.Utils
{
    public class Dy2018Util
    {
        #region List
        /// <summary>
        /// 解析html
        /// </summary>
        /// <param name="spiderPage"></param>
        /// <returns></returns>
        public static List<Dy2018Model> ParseListHtml(Page spiderPage)
        {
            var items = spiderPage.Selectable.SelectList(Selectors.XPath("//table[@class='tbspan']")).Nodes();

            var models = new List<Dy2018Model>();

            foreach (var item in items)
            {
                // //*[@id="header"]/div/div[3]/div[6]/div[2]/div[2]/div[2]/ul/table[1]/tbody/tr[2]/td[2]/b/a

                bool isSecondATag = item.Select(Selectors.XPath(".//tr[2]/td[2]/b/a[2]")).Nodes().Count() > 0 ? true : false;
                var title = isSecondATag ? item.Select(Selectors.XPath(".//tr[2]/td[2]/b/a[2]")).GetValue() : item.Select(Selectors.XPath(".//tr[2]/td[2]/b/a")).GetValue();
                var url = isSecondATag ? item.Select(Selectors.XPath(".//tr[2]/td[2]/b/a[2]")).Links().GetValue() : item.Select(Selectors.XPath(".//tr[2]/td[2]/b/a")).Links().GetValue().Trim();
                var summary = item.Select(Selectors.XPath(".//tr[4]/td[1]")).GetValue();

                RegionEnum? countryEnum = GetCountryEnum(summary);
                if (countryEnum == null)
                {
                    countryEnum = GetCountryEnumFromUrl(spiderPage.TargetUrl);
                }
                if (countryEnum == null)
                {
                    countryEnum = RegionEnum.China;
                }

                var blog = new Dy2018Model
                {
                    Country = countryEnum.Value,
                    Title = HtmlUtil.RemoveHTMLTag(title),
                    Url = url
                };

                models.Add(blog);
            }

            return models;
        }

        public static RegionEnum? GetCountryEnum(string summary)
        {
            /*
              ◎译    名　星际迷航3：超越星辰/星际迷航13：超越/星际旅行13：超越太空/星空奇遇记13：超域时空(港)/星际争霸战13：浩瀚无垠(台)/星舰奇航记13/星舰迷航记13◎片　　名　Star Trek Beyond◎年　　代　2016◎国　　家　美国◎类　　别　动作/科幻/冒险◎语　　言　英语◎字　　幕　中文字幕◎上映日期　2016-09-02(中国大陆) / 2016-07-22(美国)◎豆瓣评分　7.5/10          
              ◎译　　名　机械师2：复活/极速秒杀2(台)/机械师2/秒速杀机2(港)◎片　　名　Mechanic: Resurrection◎年　　代　2016◎国　　家　法国/美国◎类　　别　动作/犯罪/惊悚◎语　　言　英语/保加利亚语◎字　　幕　中文字幕◎上映日期　2016-10-21(中国大陆) / 2016-08-26(美国) / 2016-08-31(法国)◎豆瓣评分　6.0/10 from 636 users◎IMDb评分　5.9/10 from 9794 users◎文件格
             */

            RegionEnum? countryEnum = null;

            summary = Regex.Replace(summary, @"\s", "");

            Regex regex = new Regex(@"◎国家([\w+/*]+)◎");

            Match match = regex.Match(summary);
            if (match.Success)
            {
                var country = match.Value.Replace("国家", "").Replace("◎", "");

                var isChina = CountryUtil.IsChina(country);

                if (CountryUtil.IsChina(country))
                {
                    countryEnum = RegionEnum.China;
                }
                else if (CountryUtil.IsJapanOrKoreaa(country))
                {
                    countryEnum = RegionEnum.JapanOrKorea;
                }
                else if (CountryUtil.IsEuropeOrAmerica(country))
                {
                    countryEnum = RegionEnum.EuropeOrAmerica;
                }
            }

            return countryEnum;
        }

        /*
         * 最新 http://www.dy2018.com/html/gndy/dyzz/index.html
         * 国内 http://www.dy2018.com/html/gndy/china/index.html
         * 欧美 http://www.dy2018.com/html/gndy/oumei/index.html
         * 日韩 http://www.dy2018.com/html/gndy/rihan/index.html                
         */
        public static RegionEnum? GetCountryEnumFromUrl(string targetUrl)
        {
            RegionEnum? countryEnum = RegionEnum.China;

            targetUrl = targetUrl.ToLower();
            Regex regex = new Regex(@"http://" + AppSetting.Dy2018Domain + @"/html/gndy/(\w+)/index(_\d+)*\.html");

            var match = regex.Match(targetUrl);

            if (match.Success)
            {
                switch (match.Groups[1].Value)
                {
                    case "china":
                        countryEnum = RegionEnum.China;
                        break;
                    case "oumei":
                        countryEnum = RegionEnum.EuropeOrAmerica;
                        break;
                    case "rihan":
                        countryEnum = RegionEnum.JapanOrKorea;
                        break;
                    case "dyzz":
                        countryEnum = null;
                        break;
                }
            }

            return countryEnum;
        }

        #endregion

        #region Detail

        /// <summary>
        /// 解析详情页html
        /// </summary>
        public static Movie ParseDetailHtml(Page spiderPage)
        {
            var movie = (Movie)spiderPage.Request.GetExtra(spiderPage.TargetUrl);

            // 发布时间
            // xpath: //div[@class='position']/span[@class='updatetime'] 
            var createDateStr = spiderPage.Selectable.Select(Selectors.XPath("//div[@class='position']/span[@class='updatetime']")).GetValue().Trim(); // 发布时间：2016-06-13
            var createDate = GetDate(createDateStr);

            // //*[@id="Zoom"]
            var detailNode = spiderPage.Selectable.Select(Selectors.XPath("//*[@id=\"Zoom\"]"));

            /*
                <p>◎译　　名　机械师2：复活/极速秒杀2(台)/机械师2/秒速杀机2(港)</p>
                <p>◎上映日期　2016-10-21(中国大陆) / 2016-08-26(美国) / 2016-08-31(法国)</p>
                <p>◎片　　名　Mechanic: Resurrection</p>
                <p>◎简　　介</p>
                <p>　　拍摄于2011年的《机械师》是杰森·斯坦森的代表作，该片翻拍自1972年的同名电影，备受动作片影迷的喜爱。</p>
                <p>　　续集《机械师2：复活》讲述了顶级杀手亚瑟（杰森·斯坦森饰）被迫再度从事刺客工作，他必须完成一系列不可能的刺杀任务，而他的对手是世界上最危险的人。本以为他已经逃离了以前危险的生活，就此消失，但是某个人居然找到了他，并绑架了他所深爱的女人。为了他和他的爱人能够逃脱，亚瑟必须完成一系列暗杀任务，名单上的人则是世界上头号危险人物。</p>
                <p>◎影片截图</p>
                <p><img src="http://tu.23juqing.com/d/file/html/gndy/dyzz/2016-09-28/dbe487185720b384fad632b92304a1ad.jpg" alt="88567.jpg" width="926" height="857"></p>
             */
            var tagPs = detailNode.SelectList(Selectors.XPath(".//p")).Nodes();

            movie.CreateTime = createDate.HasValue ? createDate.Value : movie.CreateTime;
            movie.Detail = GetDetail(detailNode);

            if (ContainsTagSpan(detailNode))
            {
                movie.OtherCnNames = GetOldNodeVal(tagPs, "译名");
                movie.EnName = GetOldNodeVal(tagPs, "片名");
                movie.PremiereDateMulti = GetNodeVal(tagPs, "上映日期");
                movie.PremiereDate = GetPremiereDate(movie.PremiereDateMulti);
                movie.Summary = GetOldNodeVal(tagPs, "简介");
            }
            else
            {
                movie.OtherCnNames = GetNodeVal(tagPs, "译名");
                movie.EnName = GetNodeVal(tagPs, "片名");
                movie.PremiereDateMulti = GetNodeVal(tagPs, "上映日期");
                movie.PremiereDate = GetPremiereDate(movie.PremiereDateMulti);
                movie.Summary = GetSummary(tagPs);
            }

            return movie;
        }

        /// <summary>
        /// 处理详情, 删除 <center><font color="#ff000">请把www.dy2018.com分享给你的朋友,更多人使用，速度更快 电影天堂www.dy2018.com欢迎你每天来!</font></center>
        /// </summary>
        public static string GetDetail(ISelectable detailNode)
        {
            var detail = string.Empty;

            var tagCenters = detailNode.SelectList(Selectors.XPath(".//center")).Nodes();
            if (tagCenters.Count > 0)
            {
                var shareCenter = tagCenters.Where(t => t.GetValue().Contains("电影天堂")).FirstOrDefault();

                if (shareCenter != null)
                {
                    detail = detailNode.GetValue().Replace(shareCenter.GetValue(), "");
                }
            }

            return detail;
        }

        /// <summary>
        /// 判断是否包含 <span style="FONT-SIZE: 12px">, 是则为旧格式(2013年以前)数据, 否则为新格式(2013年以后)数据
        /// </summary>
        public static bool ContainsTagSpan(ISelectable detailNode)
        {
            var nodeVal = RegexUtil.ReplaceSpaceTabNewline(detailNode.GetValue());

            return nodeVal.ToLower().Contains("style=\"font-size:12px\"") || nodeVal.ToLower().Contains("style='font-size:12px'");
        }

        /// <summary>
        /// 2016-10-21(中国大陆) / 2016-08-26(美国) / 2016-08-31(法国), 取最小的日期: 2016-08-26(美国)
        /// </summary>
        public static DateTime? GetPremiereDate(string premiereDateMulti)
        {
            if (!string.IsNullOrEmpty(premiereDateMulti))
            {
                Regex regex = new Regex(@"((\d{4})(-|/)(\d{1,2})(-|/)(\d{1,2}))");
                MatchCollection matches = regex.Matches(premiereDateMulti);
                if (matches.Count > 0)
                {
                    List<DateTime> dates = new List<DateTime>();
                    for (var i = 0; i < matches.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(matches[i].Value))
                        {
                            var date = Convert.ToDateTime(matches[i].Value);
                            dates.Add(date);
                        }
                    }

                    if (dates.Count > 0)
                    {
                        return dates.Min(d => d);
                    }
                }
            }

            return null;
        }

        #region 2013年以后的数据
        /*
          <div id="Zoom">
            <!--Content Start-->
            <p>&nbsp;<img src="http://tu.23juqing.com/d/file/html/gndy/dyzz/2016-09-20/94d0f9b9424d9bd48b70e96aa8f62917.jpg" alt="霓虹牛.jpg" width="638" height="946"></p>
            <p>◎译　　名　霓虹牛</p>
            <p>◎片　　名　Boi Neon</p>
            <p>◎年　　代　2015</p>
            <p>◎国　　家　巴西/乌拉圭/荷兰</p>
            <p>◎类　　别　剧情</p>
            <p>◎语　　言　葡萄牙语</p>
            <p>◎字　　幕　中文字幕</p>
            <p>◎上映日期　2015-09-03(威尼斯电影节)</p>
            <p>◎豆瓣评分　6.8/10 from 139 users</p>
            <p>◎IMDb评分　6.9/10 from 927 users</p>
            <p>◎文件格式　x264 + ACC</p>
            <p>◎视频尺寸　1280 x 720</p>
            <p>◎文件大小　1121MB</p>
            <p>◎片　　长　104分钟</p>
            <p>◎导　　演　Gabriel Mascaro</p>
            <p>◎主　　演　Juliano Cazarré</p>
            <p>　　　　　　Maeve Jinkings</p>
            <p>◎简　　介</p>
            <p>　　巴西東北的草原邊，日日上演暴烈的牛仔競技戲碼。男人負責照顧後台牛隻，女人身兼情色秀場舞孃，白天在塵土黃沙中揮汗，夜裡飽漲情慾開始橫流。他們共同養育小女孩卡卡，以卡車和道路為家，遊牧到下一個演出地點。當地正蓬勃發展時尚紡織工廠，閃亮亮片、精緻布料帶來新的野心，卻也威脅著他們的邊緣生存，孤單的靈魂、生活的未來想像，都在原始慾望的漩渦中四處衝撞，掙扎著尋找出口。</p>
            <p>◎幕后制作</p>
            <p>干净的生活流电影，几乎没什么都市的痕迹。导演用欧洲的文艺驾驭着巴西的野性，色彩运用得尤其惊艳，几乎每个镜头都有醒目的主色调提携。迷恋缝纫的男主用自己的方式定义着猛男的气质，最后的一场情欲戏画面冲击力十足～</p>
            <p>◎影片截图</p>
            <p><img src="http://tu.23juqing.com/d/file/html/gndy/dyzz/2016-09-20/9dbffc5fefb07e3018c6022ea8502fa3.jpg" alt="霓虹牛BD中英双字.rmvb_thumbs_2016.09.20.18_25_50.jpg" width="926" height="857"></p><!--duguPlayList Start-->
            <!--xunleiDownList Start-->
            <p>&nbsp;</p>
            <p style="margin: 0px; padding: 0px; color: rgb(24, 55, 120); font-family: Verdana, Arial, Helvetica, sans-serif;"><font color="#ff0000"><strong><font size="4">【迅雷下载地址】 </font></strong></font></p>
            <p style="margin: 0px; padding: 0px; color: rgb(24, 55, 120); font-family: Verdana, Arial, Helvetica, sans-serif;">&nbsp;</p>
            <table style="BORDER-BOTTOM: #cccccc 1px dotted; BORDER-LEFT: #cccccc 1px dotted; TABLE-LAYOUT: fixed; BORDER-TOP: #cccccc 1px dotted; BORDER-RIGHT: #cccccc 1px dotted" border="0" cellspacing="0" cellpadding="6" width="95%" align="center">
                <tbody>
                    <tr>
                        <td style="WORD-WRAP: break-word" bgcolor="#fdfddf"><anchor><a target="_self" href="#" title="迅雷专用高速下载" gdxkpxju="thunder://QUFmdHA6Ly9hOmFAZHlnb2QxOC5jb20vWyVFNyU5NCVCNSVFNSVCRCVCMSVFNSVBNCVBOSVFNSVBMCU4Mnd3dy5keTIwMTguY29tXSVFOSU5QyU5MyVFOCU5OSVCOSVFNyU4OSU5QkJEJUU0JUI4JUFEJUU4JThCJUIxJUU1JThGJThDJUU1JUFEJTk3LnJtdmJaWg==" thunderpid="00000" thundertype="" thunderrestitle="" onclick="return OnDownloadClick_Simple(this,2)" oncontextmenu="ThunderNetwork_SetHref(this)">ftp://a:a@dygod18.com:21/[电影天堂www.dy2018.com]霓虹牛BD中英双字.rmvb</a>&nbsp;&nbsp;</anchor></td>
                    </tr>
                </tbody>
            </table>
            <p style="margin: 0px; padding: 0px; color: rgb(24, 55, 120); font-family: Verdana, Arial, Helvetica, sans-serif;">&nbsp;</p>  
            <script type="text/javascript">if(goPAGE()=="win"){document.writeln("<SCRIPT src='/jsdd/750.js'></SCR"+"IPT>")}</script><script src="/jsdd/750.js"></script>
            <br>      
            <hr color="#CC6600" size="1px">
            <center><font color="#ff000">请把www.dy2018.com分享给你的朋友,更多人使用，速度更快 电影天堂www.dy2018.com欢迎你每天来!</font></center>
          </div>
         */


        /// <summary>
        /// <p>◎译　　名　机械师2：复活/极速秒杀2(台)/机械师2/秒速杀机2(港)</p>
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="name">译名</param>
        /// <returns>机械师2：复活/极速秒杀2(台)/机械师2/秒速杀机2(港)</returns>
        private static string GetNodeVal(IList<ISelectable> nodes, string name)
        {
            name = name.Trim();
            if (!name.StartsWith("◎"))
            {
                name = "◎" + name;
            }
            var node = nodes.Where(n => RegexUtil.ReplaceSpaceTabNewline(HtmlUtil.RemoveHTMLTag(n.GetValue())).Contains(name)).FirstOrDefault();
            if (node != null)
            {
                var nodeVal = RegexUtil.ReplaceSpaceTabNewline(HtmlUtil.RemoveHTMLTag(node.GetValue()));
                return nodeVal.Replace(name, "");
            }
            else
            {
                return null;
            }
        }

        /*
            <p>◎简　　介</p>
            <p>　　拍摄于2011年的《机械师》是杰森·斯坦森的代表作，该片翻拍自1972年的同名电影，备受动作片影迷的喜爱。</p>
            <p>　　续集《机械师2：复活》讲述了顶级杀手亚瑟（杰森·斯坦森饰）被迫再度从事刺客工作，他必须完成一系列不可能的刺杀任务，而他的对手是世界上最危险的人。本以为他已经逃离了以前危险的生活，就此消失，但是某个人居然找到了他，并绑架了他所深爱的女人。为了他和他的爱人能够逃脱，亚瑟必须完成一系列暗杀任务，名单上的人则是世界上头号危险人物。</p>
            <p>◎影片截图</p>
            <p><img src="http://tu.23juqing.com/d/file/html/gndy/dyzz/2016-09-28/dbe487185720b384fad632b92304a1ad.jpg" alt="88567.jpg" width="926" height="857"></p>
        */
        private static string GetSummary(IList<ISelectable> nodes)
        {
            string summary = string.Empty;

            //取[◎简介]和[◎影片截图]之间最多两个 P 标签
            var containsSummary = false;
            var pCount = 0;
            var maxCount = 2;
            for (var i = 0; i < nodes.Count; i++)
            {
                var nodeVal = RegexUtil.ReplaceSpaceTabNewline(HtmlUtil.RemoveHTMLTag(nodes[i].GetValue()));

                if (pCount >= maxCount || nodeVal.Contains("◎影片截图"))
                {
                    break;
                }

                if (containsSummary && !string.IsNullOrEmpty(nodeVal) && pCount < maxCount)
                {
                    pCount++;

                    summary += nodeVal;
                }

                if (nodeVal.Contains("◎简介"))
                {
                    containsSummary = true;
                }
            }

            return summary;
        }

        #endregion

        #region 2013年以前的数据
        /*
           <div id="Zoom">
            <!--Content Start-->
            <span style="FONT-SIZE: 12px">
                <p>
                    <br>
                    <img border="0" src="http://img.23juqing.com/tupian/77380-66548034201306101617113106160106372_003.jpg" alt="">
                    <br><br>
                    ◎译　　名　钢琴木马/钢琴密码/钢琴密码之骇客公敌<br>
                    ◎片　　名　Piano Trojan<br>
                    ◎年　　代　2013<br>
                    ◎国　　家　中国<br>
                    ◎类　　别　剧情/爱情/悬疑<br>
                    ◎语　　言　普通话<br>
                    ◎字　　幕　中文<br>
                    ◎文件格式　HD-RMVB<br>
                    ◎视频尺寸　1024 x 576<br>
                    ◎文件大小　1CD<br>
                    ◎片　　长　86 min<br>
                    ◎导　　演　祖松 Zusong Lv<br>
                    ◎主　　演　张默 Mo Zhang&nbsp; ....王阿明<br>　　　　　　林逸欣 Shara Lin&nbsp; ....李清子<br>　　　　　　尤秋兴 Qiuxing You&nbsp; ....杀手<br>　　　　　　颜志琳 Zhilin Yan&nbsp; ....杀手<br>　　　　　　连凯 Hoi Lin&nbsp; ....黄灿森<br>　　　　　　安泽豪 Zehao An&nbsp; ....徐东辉<br>　　　　　　金鹏 Peng Jin<br>　　　　　　刘通 Liu Tong<br>　　　　　　张铮 Zheng Zhang&nbsp; ....李莱雅<br><br>
                    ◎简　　介　<br><br>　　1949年，全国解放前夕，作为国民政府逃台的最后一站，厦门上演着惨烈的生死谍战。年轻的国民党军上尉黄灿森（连凯 饰）深深爱上了教会女子学院的钢琴教师李莱亚（张铮饰）......
                    <br><br><img border="0" src="http://img.23juqing.com/tupian/77380-66548034201306101617113106160106372_001.jpg" alt="">
                </p>
                <p><font color="#ff0000"><strong><font size="4">【下载地址】</font></strong></font></p>
                <p><strong><font color="#ff0000" size="4"></font></strong>&nbsp;</p>
                <p><strong><font color="#ff0000" size="4"></font></strong>&nbsp;
                </p><table style="BORDER-BOTTOM: #cccccc 1px dotted; BORDER-LEFT: #cccccc 1px dotted; TABLE-LAYOUT: fixed; BORDER-TOP: #cccccc 1px dotted; BORDER-RIGHT: #cccccc 1px dotted" border="0" cellspacing="0" cellpadding="6" width="95%" align="center">
                    <tbody>
                        <tr>
                            <td style="WORD-WRAP: break-word" bgcolor="#fdfddf"><anchor><a target="_self" href="#" title="迅雷专用高速下载" wcfviqqu="thunder://QUFmdHA6Ly9keWdvZDE6ZHlnb2QxQGQwMjMuZHlkeXR0LmNvbTo4MDA1L1slRTclOTQlQjUlRTUlQkQlQjElRTUlQTQlQTklRTUlQTAlODJ3d3cuZHkyMDE4Lm5ldF0uJUU5JTkyJUEyJUU3JTkwJUI0JUU2JTlDJUE4JUU5JUE5JUFDLkJELjEwMjR4NTc2LiVFNSU5QiVCRCVFOCVBRiVBRCVFNCVCOCVBRCVFNSVBRCU5Ny5ybXZiWlo=" thunderpid="00000" thundertype="" thunderrestitle="" onclick="return OnDownloadClick_Simple(this,2)" oncontextmenu="ThunderNetwork_SetHref(this)">ftp://dygod1:dygod1@d023.dydytt.com:8005/[电影天堂www.dy2018.net].钢琴木马.BD.1024x576.国语中字.rmvb</a>&nbsp;&nbsp;</anchor></td>
                        </tr>
                    </tbody>
                </table>
                <p></p> <br>
                <center></center>
                  
                <!--duguPlayList Start-->
                <!--xunleiDownList Start-->      
                <script type="text/javascript">if(goPAGE()=="win"){document.writeln("<SCRIPT src='/jsdd/750.js'></SCR"+"IPT>")}</script><script src="/jsdd/750.js"></script>
                <br>      
                <hr color="#CC6600" size="1px">      

                <center><font color="#ff000">请把www.dy2018.com分享给你的朋友,更多人使用，速度更快 电影天堂www.dy2018.com欢迎你每天来!</font></center>
            </span>
           </div>
         */

        /// <summary>
        /// 2013年前的数据, 取第一个 P标签数据
        /// </summary>
        /// <param name="nodes">所有P标签</param>
        /// <param name="name">译名/片名/年代/简介 等</param>
        /// <returns></returns>
        private static string GetOldNodeVal(IList<ISelectable> nodes, string name)
        {
            string val = null;

            name = name.Trim();

            //◎译名钢琴木马/钢琴密码/钢琴密码之骇客公敌◎片名PianoTrojan◎年代2013
            var nodeVal = RegexUtil.ReplaceSpaceTabNewline(HtmlUtil.RemoveHTMLTag(nodes[0].GetValue()));

            var vals = nodeVal.Split('◎');
            for (var i = 0; i < vals.Length; i++)
            {
                if (vals[i].StartsWith(name))
                {
                    val = vals[i].Remove(0, name.Length);
                    break;
                }
            }

            return val;
        }

        #endregion

        public static DateTime? GetDate(string dateStr)
        {
            Regex regex = new Regex(@"((\d{4})(-|/)(\d{1,2})(-|/)(\d{1,2}))");
            var match = regex.Match(dateStr);
            if (match.Success)
            {
                return Convert.ToDateTime(match.Value);
            }

            return null;
        }

        #endregion
    }
}

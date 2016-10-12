using DotnetSpider.Core;
using DotnetSpider.Core.Monitor;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Core.Selector;
using MovieSpider.Consts;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace MovieSpider.Spiders
{
    public class Dy2018Spider
    {
        public static void Run()
        {
            // 注入监控服务
            IocContainer.Default.AddSingleton<IMonitor, NLogMonitor>();

            // 定义要采集的 Site 对象, 可以设置 Header、Cookie、代理等
            var site = new Site { EncodingName = CommonConst.UTF8 };
            site.AddStartUrl(AppSetting.Dy2018Url);

            // 使用内存Scheduler、自定义PageProcessor、自定义Pipeline创建爬虫
            Spider spider = Spider.Create(site, new Dy2018Processor(), new QueueDuplicateRemovedScheduler()).AddPipeline(new Dy2018Pipeline()).SetThreadNum(2);
            spider.EmptySleepTime = 10 * 1000;
            // 注册爬虫到监控服务
            MonitorCenter.Register(spider);

            // 启动爬虫
            spider.Run();

            //Console.Read();
        }

        private class Dy2018Processor : IPageProcessor
        {
            public Site Site { get; set; }

            public void Process(Page page)
            {
                var items = page.Selectable.SelectList(Selectors.XPath("//div[@class='post_item_body']")).Nodes();

                var blogs = new List<Dy2018>();
                foreach (var item in items)
                {
                    var summaryText = item.Select(Selectors.XPath(".//p[@class='post_item_summary']/a/following-sibling::text()")).GetValue() != null
                        ? item.Select(Selectors.XPath(".//p[@class='post_item_summary']/a/following-sibling::text()")).GetValue()
                        : item.Select(Selectors.XPath(".//p[@class='post_item_summary']")).GetValue();

                    var blog = new Dy2018
                    {
                        Title = item.Select(Selectors.XPath(".//h3/a")).GetValue().Trim(),
                        Summary = summaryText.Trim(),
                        Url = item.Select(Selectors.XPath(".//h3/a")).Links().GetValue().Trim(),
                        Author = item.Select(Selectors.XPath(".//div[@class='post_item_foot']/a")).GetValue().Trim()
                    };

                    blogs.Add(blog);
                }


                // 以自定义KEY存入page对象中供Pipeline调用
                page.AddResultItem(CommonConst.SpiderResult, blogs);
            }
        }

        private class Dy2018Pipeline : BasePipeline
        {
            public override void Process(ResultItems resultItems)
            {
                foreach (Dy2018 dy2018 in resultItems.Results[CommonConst.SpiderResult])
                {
                    File.AppendAllLines("Dy2018.txt", new[] { dy2018.Title, " ", dy2018.Summary, " ", dy2018.Url, " ", dy2018.Author, " " });
                }

                // 可以自由实现插入数据库或保存到文件
            }
        }

        public class Dy2018
        {
            public string Title { get; set; }

            public string Summary { get; set; }

            public string Url { get; set; }

            public string Author { get; set; }
        }
    }
}

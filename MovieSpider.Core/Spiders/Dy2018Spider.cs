﻿using DotnetSpider.Core;
using DotnetSpider.Core.Monitor;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Core.Selector;
using MovieSpider.Consts;
using MovieSpider.Core.Consts;
using MovieSpider.Core.Models;
using MovieSpider.Core.Utils;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace MovieSpider.Core.Spiders
{
    public class Dy2018Spider
    {
        public static void Run(List<string> urls)
        {
            // 注入监控服务
            IocContainer.Default.AddSingleton<IMonitor, NLogMonitor>();

            // 定义要采集的 Site 对象, 可以设置 Header、Cookie、代理等
            var site = new Site { EncodingName = AppSetting.Dy2018Encode };

            site.AddStartUrls(urls);

            // 使用内存Scheduler、自定义PageProcessor、自定义Pipeline创建爬虫
            Spider spider = Spider.Create(site, new Dy2018Processor(), new QueueDuplicateRemovedScheduler()).AddPipeline(new Dy2018Pipeline()).SetThreadNum(1);
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
                var models = Dy2018Util.ParseHtml(page);

                // 以自定义KEY存入page对象中供Pipeline调用
                page.AddResultItem(CommonConst.SpiderResult, models);
            }
        }

        private class Dy2018Pipeline : BasePipeline
        {
            public override void Process(ResultItems resultItems)
            {
                foreach (Dy2018Model model in resultItems.Results[CommonConst.SpiderResult])
                {
                    File.AppendAllLines("Dy2018.txt", new[] { model.Title, model.Url, model.Country.ToString() });
                }

                // 可以自由实现插入数据库或保存到文件
            }
        }
    }
}
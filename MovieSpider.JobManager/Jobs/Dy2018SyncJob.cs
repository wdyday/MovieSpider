﻿using MovieSpider.Core;
using MovieSpider.Core.Consts;
using MovieSpider.Core.Ioc;
using MovieSpider.Services;
using Newtonsoft.Json;
using NLog;
using Quartz;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.JobManager.Jobs
{
    public class Dy2018SyncJob : IJob
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _movieDomain = ConfigurationManager.AppSettings["MovieDomain"];

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Dy2018SyncJob Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("Dy2018SyncJob Start! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));

            try
            {
                var movieService = Ioc.Get<IMoviceService>();

                var movies = movieService.GetTopNotSyncMovies(CommonConst.TopCount);

                if (movies.Count > 0)
                {
                    RestClient client = new RestClient(_movieDomain);

                    var request = new RestRequest("api/Movie/AddMovies", Method.POST);
                    request.AddJsonBody(movies);

                    var response = client.Execute(request);

                    var result = JsonConvert.DeserializeObject<ResponseResult>(response.Content);
                    if (result.Success)
                    {
                        var movieIds = movies.Select(m => m.MovieId).ToList();
                        movieService.UpdateSyncDone(movieIds);
                    }
                    else
                    {
                        _logger.Info(result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }

            Console.WriteLine("Dy2018SyncJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
            _logger.Info("Dy2018SyncJob End! " + DateTime.Now.ToString(CommonConst.DateFormatYmdhms));
        }
    }
}
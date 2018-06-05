using MovieSpider.Core;
using MovieSpider.Core.Crypto;
using MovieSpider.Core.Utils;
using MovieSpider.Core.Extentions;
using MovieSpider.Data.Entities;
using MovieSpider.Data.IContentEntities;
using MovieSpider.Data.Models;
using Newtonsoft.Json;
using NLog;
using Omu.ValueInjecter;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MovieSpider.JobManager.Utils
{
    public class RestUtils
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _movieDomain = ConfigurationManager.AppSettings["MovieDomain"];
        private string _apiKey = ConfigurationManager.AppSettings["ApiKey"];

        private string Token = null;
        private string QueryString
        {
            get
            {
                return $"apikey={HttpUtility.UrlEncode(_apiKey)}"
                       + $"&timestamp={DateUtil.ConvertToTimestamp(DateTime.Now)}"
                       + $"&token={HttpUtility.UrlEncode(Token)}"
                       + $"&signature={HttpUtility.UrlEncode(CryptoUtils.ComputeHash(_apiKey + Token))}";
            }
        }

        private string GetResource(string resource)
        {
            return $"{resource}?{QueryString}";
        }

        public RestUtils()
        {
            this.Token = GetToken();
        }

        public string GetToken()
        {
            RestClient client = new RestClient(_movieDomain);

            var request = new RestRequest($"api/Token/GetToken/ApiKey@{_apiKey}", Method.GET);
            var response = client.Execute(request);

            var result = JsonConvert.DeserializeObject<ResponseResult>(response.Content);

            if (!result.Success)
            {
                _logger.Info($"[GetToken] - {JsonUtil.JsonToString(result)}");
            }

            return result.Success ? result.Data.ToString() : null;
        }

        private bool CheckToken(string from)
        {
            //_logger.Info($"[{from}] - {QueryString}");

            if (string.IsNullOrEmpty(Token))
            {
                _logger.Info($"[{from}] - Null Token");

                return false;
            }
            return true;
        }

        #region Dy2018SyncJob

        /// <summary>
        /// 抓取movie同步至网站
        /// </summary>
        /// <param name="movies"></param>
        /// <returns></returns>
        public ResponseResult SaveMovies(List<Movie> movies)
        {
            if (!CheckToken("SaveMovies"))
            {
                return new ResponseResult(false, "Null Token");
            }
            try
            {
                RestClient client = new RestClient(_movieDomain);

                var request = new RestRequest(GetResource("api/Movie/SaveMovies"), Method.POST);
                request.AddJsonBody(movies);

                var response = client.Execute(request);

                var result = JsonConvert.DeserializeObject<ResponseResult>(response.Content);

                return result;
            }
            catch(Exception ex)
            {
                return new ResponseResult(false, ex.Message);
            }
        }

        #endregion

        #region PostSyncBakJob
        /// <summary>
        /// 同步备份至新库
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public ResponseResult SyncBakPosts(List<Post> posts)
        {
            if (!CheckToken("SyncBakPosts"))
            {
                return new ResponseResult(false, "Null Token");
            }

            var postModels = new List<PostModel>();
            foreach (var post in posts)
            {
                var postModel = new PostModel { Comments = new List<CommentModel>() };
                postModel.InjectFrom(post);
                postModel.Comments.InjectFrom(post.Comments);
                postModels.Add(postModel);
            }

            RestClient client = new RestClient(_movieDomain);

            var request = new RestRequest(GetResource("api/Movie/SyncPostsAndComments"), Method.POST);
            request.AddJsonBody(postModels);

            var response = client.Execute(request);

            var result = JsonConvert.DeserializeObject<ResponseResult>(response.Content);

            return result;
        }

        #endregion

        #region PostSyncJob  网站后台修改的数据同步给 本地抓取库

        public int GetNotSyncPostCount()
        {
            if (!CheckToken("GetNotSyncPostCount"))
            {
                return 0;
            }

            int count = 0;

            try
            {
                RestClient client = new RestClient(_movieDomain);

                var request = new RestRequest(GetResource("api/Movie/GetNotSyncPostCount"), Method.POST);
                //request.AddJsonBody(null);

                var response = client.Execute(request);

                var result = JsonConvert.DeserializeObject<ResponseResult>(response.Content);
                if (result.Success)
                {
                    count = Convert.ToInt32(result.Data);
                }
                else
                {
                    _logger.Info("[GetNotSyncPostCount] - " + result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }

            return count;
        }


        /// <summary>
        /// 分页取未同步是数据
        /// </summary>
        public List<PostModel> GetNotSyncPosts(int pageIndex, int pageSize)
        {
            if (!CheckToken("GetNotSyncPosts"))
            {
                return new List<PostModel>();
            }

            List<PostModel> movies = new List<PostModel>();

            try
            {
                RestClient client = new RestClient(_movieDomain);

                var body = new
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                var request = new RestRequest(GetResource("api/Movie/GetNotSyncPosts"), Method.POST);
                request.AddJsonBody(body);

                var response = client.Execute(request);

                var result = JsonConvert.DeserializeObject<ResponseResult>(response.Content);
                if (result.Success)
                {
                    movies = JsonConvert.DeserializeObject<List<PostModel>>(result.Data.ToString());
                }
                else
                {
                    _logger.Info("[GetNotSyncPosts] - " + result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }

            return movies;
        }

        /// <summary>
        /// 更新 IsSyncDone
        /// </summary>
        /// <param name="postIds"></param>
        /// <returns></returns>
        public void UpdateIsSyncDone(List<int> postIds)
        {
            if (!CheckToken("UpdateIsSyncDone"))
            {
                return;
            }

            try
            {
                RestClient client = new RestClient(_movieDomain);

                var request = new RestRequest(GetResource("api/Movie/UpdateIsSyncDone"), Method.POST);
                request.AddJsonBody(postIds);

                var response = client.Execute(request);

                var result = JsonConvert.DeserializeObject<ResponseResult>(response.Content);
                if (!result.Success)
                {
                    _logger.Info("[UpdateIsSyncDone] - " + result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }
        }

        #endregion
    }
}

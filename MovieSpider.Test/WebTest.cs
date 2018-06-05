using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieSpider.Core;
using MovieSpider.Core.Extentions;
using MovieSpider.Data.Entities;
using MovieSpider.Data.Models;
using MovieSpider.JobManager.Utils;
using MovieSpider.Services;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using RestSharp;
using System.Collections.Generic;
using System.Configuration;

namespace MovieSpider.Test
{
    [TestClass]
    public class WebTest
    {
        private string _movieDomain = ConfigurationManager.AppSettings["MovieDomain"];

        [TestMethod]
        public void PostCommentSyncApiTest()
        {
            var postModels = new List<PostModel>();
            var posts = new PostService().GetPostsWithComments(1, 10);
            foreach (var post in posts)
            {
                var postModel = new PostModel { Comments = new List<CommentModel>() };
                postModel.InjectFrom(post);
                postModel.Comments.InjectFrom(post.Comments);
                postModels.Add(postModel);
            }

            RestClient client = new RestClient(_movieDomain);

            var request = new RestRequest("api/Movie/SyncPostsAndComments", Method.POST);
            request.AddJsonBody(postModels);

            var response = client.Execute(request);

            var result = JsonConvert.DeserializeObject<ResponseResult>(response.Content);
        }


        [TestMethod]
        public void SaveMoviesTest()
        {
            var movies = new List<Movie>();
            var movie = new MoviceService().Get(37895);
            movies.Add(movie);
            
            var restUtils = new RestUtils();

            var response = restUtils.SaveMovies(movies);

            Assert.IsTrue(response.Success);
        }
    }
}

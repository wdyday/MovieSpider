using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieSpider.Core;
using MovieSpider.Core.Extentions;
using MovieSpider.Data.Models;
using MovieSpider.Services.Implementations;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using RestSharp;
using System.Collections.Generic;

namespace MovieSpider.Test
{
    [TestClass]
    public class WebTest
    {
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

            RestClient client = new RestClient("http://localhost:24153/");

            var request = new RestRequest("api/Movie/SyncPostsAndComments", Method.POST);
            request.AddJsonBody(postModels);

            var response = client.Execute(request);

            var result = JsonConvert.DeserializeObject<ResponseResult>(response.Content);
        }
    }
}

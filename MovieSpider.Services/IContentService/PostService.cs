using MovieSpider.Core;
using MovieSpider.Core.Utils;
using MovieSpider.Data;
using MovieSpider.Data.DbEnums;
using MovieSpider.Data.Entities;
using MovieSpider.Data.IContentEntities;
using MovieSpider.Services.IContentCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Services
{
    public class PostService
    {
        #region 同步网站备份时使用
        public int GetPostCount()
        {
            using (var db = new IContentDbContext())
            {
                return db.Post.AsNoTracking().Count();
            }
        }

        public List<Post> GetPostsWithComments(int pageIndex, int pageSize)
        {
            using (var db = new IContentDbContext())
            {
                pageIndex = pageIndex > 0 ? pageIndex : 1;
                pageSize = pageSize > 0 ? pageSize : 50;

                var posts = db.Post
                    .Include(nameof(Post.Comments))
                    .AsNoTracking()
                    .OrderBy(p => p.PostId)
                    .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return posts;
            }
        }
        #endregion

        #region 同步抓取的数据到web数据库

        public ResponseResult SyncToWebDB(List<Movie> movies)
        {
            var result = new ResponseResult();

            try
            {
                var posts = movies.Select(m => new Post
                {
                    //IsSyncDone = true,
                    FromUrl = m.FromUrl,

                    Title = m.CnName,
                    PostContent = m.Detail,
                    Summary = m.Summary,
                    CreateTime = m.CreateTime,
                    DefaultImagePath = !string.IsNullOrEmpty(m.Detail) ? HtmlUtil.GetFirstMatchValue(MatchType.Image, m.Detail) : null,
                    Status = 1,
                    Region = m.Region,
                    PremiereDate = m.PremiereDate,
                    PremiereDateMulti = m.PremiereDateMulti,
                    CnName = m.CnName,
                    OtherCnNames = m.OtherCnNames,
                    EnName = m.EnName,
                    MediaType = GetMediaType(m.CnName, m.MediaType),
                    BoardId = AppConfigCache.Movie_Board_ID,
                    UserId = AppConfigCache.Admin_User_ID
                }).ToList();

                var fromUrls = movies.Select(m => m.FromUrl).ToList();
                var dbFromUrls = GetPostsFromUrls(fromUrls);

                var addPosts = posts.Where(p => !dbFromUrls.Contains(p.FromUrl)).ToList();
                var updatePosts = posts.Where(p => dbFromUrls.Contains(p.FromUrl)).ToList();

                AddPosts(addPosts);
                UpdatePosts(updatePosts, false);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ExceptionUtil.GetExceptionMessage(ex);
            }

            return result;
        }

        #region 本地抓取表 同步到 网站 Movie  -> Post

        /// <summary>
        /// job 同步数据时, 可能有重复数据, 用此方法排除重复数据
        /// </summary>
        /// <param name="fromUrls"></param>
        /// <returns></returns>
        public List<string> GetPostsFromUrls(List<string> fromUrls)
        {
            using (var db = new IContentDbContext())
            {
                return db.Post.AsNoTracking().Where(p => fromUrls.Contains(p.FromUrl)).Select(p => p.FromUrl).ToList();
            }
        }

        public void AddPosts(List<Post> movies)
        {
            using (var db = new IContentDbContext())
            {
                db.Post.AddRange(movies);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 更新Post
        /// </summary>
        /// <param name="movies"></param>
        /// <param name="updateIsSyncDone">
        /// IsSyncDone = false, 说明管理员修改过当前Post, 
        /// updateIsSyncDone 用来指明是否更新PostContent数据, updateIsSyncDone = true 做更新, 否则不做更新
        /// </param>
        public void UpdatePosts(List<Post> movies, bool updateIsSyncDone)
        {
            using (var db = new IContentDbContext())
            {
                var fromUrls = movies.Select(m => m.FromUrl).ToList();
                var dbPosts = db.Post.Where(p => fromUrls.Contains(p.FromUrl)).ToList();

                foreach (var post in dbPosts)
                {
                    var movie = movies.Where(m => m.FromUrl == post.FromUrl).FirstOrDefault();

                    post.FromUrl = movie.FromUrl;
                    //post.CreateTime = movie.CreateTime;
                    post.UpdateTime = DateTime.Now;
                    if (movie.Region != RegionEnum.None)
                    {
                        post.Region = movie.Region;
                    }
                    if (movie.MediaType != MediaTypeEnum.None)
                    {
                        post.MediaType = movie.MediaType;
                    }

                    if (!string.IsNullOrEmpty(movie.CnName))
                    {
                        post.CnName = movie.CnName;
                    }
                    if (!string.IsNullOrEmpty(movie.CnName))
                    {
                        post.Title = movie.CnName;
                    }

                    // IsSyncDone 非 null 时,说明管理员手动修改过数据, 此时不要更新 PostContent
                    // updateIsSyncDone 强制更新
                    if (updateIsSyncDone || !post.IsSyncDone.HasValue)
                    {
                        if (!string.IsNullOrEmpty(movie.PostContent))
                        {
                            post.PostContent = movie.PostContent;
                        };
                    }

                    if (!string.IsNullOrEmpty(movie.OtherCnNames))
                    {
                        post.OtherCnNames = movie.OtherCnNames;
                    }
                    if (!string.IsNullOrEmpty(movie.EnName))
                    {
                        post.EnName = movie.EnName;
                    }
                    if (!string.IsNullOrEmpty(movie.Summary))
                    {
                        post.Summary = movie.Summary;
                    }
                    if (!string.IsNullOrEmpty(movie.DefaultImagePath))
                    {
                        post.DefaultImagePath = movie.DefaultImagePath;
                    }
                    if (movie.PremiereDate.HasValue)
                    {
                        post.PremiereDate = movie.PremiereDate;
                    }
                    if (!string.IsNullOrEmpty(movie.PremiereDateMulti))
                    {
                        post.PremiereDateMulti = movie.PremiereDateMulti;
                    }
                }

                db.SaveChanges();
            }
        }

        #endregion

        /// <summary>
        /// 媒体类型
        /// 0: 电影, 1: 电视剧, 3: 动漫, 4: 综艺
        /// </summary>
        /// <param name="cnName">视频中文名</param>
        /// <returns></returns>
        private MediaTypeEnum GetMediaType(string cnName, MediaTypeEnum mediaType)
        {
            var type = MediaTypeEnum.None;

            if (!string.IsNullOrEmpty(cnName))
            {
                if (/*cnName.Contains("国产剧") || */cnName.Contains("港台剧") || cnName.Contains("日韩剧") || cnName.Contains("欧美剧"))
                {
                    type = MediaTypeEnum.TV;
                }
                else if (cnName.Contains("综艺"))
                {
                    type = MediaTypeEnum.Variety;
                }
                else if (cnName.Contains("动漫"))
                {
                    type = MediaTypeEnum.Cartoon;
                }
            }

            if (type == MediaTypeEnum.None)
            {
                type = (mediaType != MediaTypeEnum.None) ? mediaType : MediaTypeEnum.Movie;
            }

            return type;
        }

        #endregion
    }
}

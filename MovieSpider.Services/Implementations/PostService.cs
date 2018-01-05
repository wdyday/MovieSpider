using MovieSpider.Data;
using MovieSpider.Data.IContentEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Services.Implementations
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
    }
}

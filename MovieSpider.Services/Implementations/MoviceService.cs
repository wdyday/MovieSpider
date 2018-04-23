using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieSpider.Data.Entities;
using MovieSpider.Core.Pager;
using MovieSpider.Data;
using MovieSpider.Data.Models;
using MovieSpider.Data.DbEnums;

namespace MovieSpider.Services
{
    public class MoviceService : IMoviceService
    {
        public Movie Get(int id)
        {
            using (var db = new SpiderDbContext())
            {
                return db.Movie.Where(m => m.MovieId == id).FirstOrDefault();
            }
        }

        public void UpdateMovieDetailDone(MovieModel movie)
        {
            using (var db = new SpiderDbContext())
            {
                var dbMovie = db.Movie.Where(m => m.MovieId == movie.MovieId).FirstOrDefault();
                if (dbMovie != null)
                {
                    dbMovie.UpdateTime = DateTime.Now;
                    dbMovie.Detail = movie.Detail;
                    dbMovie.Summary = movie.Summary;
                    dbMovie.OtherCnNames = movie.OtherCnNames;
                    dbMovie.PremiereDateMulti = movie.PremiereDateMulti;
                    dbMovie.PremiereDate = movie.PremiereDate;
                    dbMovie.JobStatus = JobStatusEnum.DetailDone;

                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 同步web数据到Movie
        /// </summary>
        /// <param name="posts"></param>
        public void UpdateMovieByWeb(List<PostModel> posts)
        {
            using (var db = new SpiderDbContext())
            {
                var fromUrls = posts.Select(p => p.FromUrl).ToList();
                var dbMovies = db.Movie.Where(m => fromUrls.Contains(m.FromUrl)).ToList();

                foreach (var dbMovie in dbMovies)
                {
                    var post = posts.Where(p => p.FromUrl == dbMovie.FromUrl).First();

                    dbMovie.CreateTime = post.CreateTime;
                    if (!string.IsNullOrEmpty(post.CnName))
                    {
                        dbMovie.CnName = post.CnName;
                    }
                    if (!string.IsNullOrEmpty(post.EnName))
                    {
                        dbMovie.EnName = post.EnName;
                    }
                    if (!string.IsNullOrEmpty(post.PostContent))
                    {
                        dbMovie.Detail = post.PostContent;
                    }
                    if (!string.IsNullOrEmpty(post.Summary))
                    {
                        dbMovie.Summary = post.Summary;
                    }
                    if (!string.IsNullOrEmpty(post.OtherCnNames))
                    {
                        dbMovie.OtherCnNames = post.OtherCnNames;
                    }
                    if (post.PremiereDate.HasValue)
                    {
                        dbMovie.PremiereDate = post.PremiereDate;
                    }
                    if (!string.IsNullOrEmpty(post.PremiereDateMulti))
                    {
                        dbMovie.PremiereDateMulti = post.PremiereDateMulti;
                    }
                    if (post.MediaType.HasValue)
                    {
                        dbMovie.MediaType = post.MediaType.Value;
                    }

                    dbMovie.IsSyncedByWeb = true;
                }

                db.SaveChanges();
            }
        }

        public void AddMovies(List<Movie> movies)
        {
            using (var db = new SpiderDbContext())
            {
                movies.ForEach(m =>
                {
                    m.JobStatus = JobStatusEnum.ListDone;
                    m.CreateTime = DateTime.Now;
                });
                db.Movie.AddRange(movies);
                db.SaveChanges();
            }
        }

        public void UpdateMovies(List<Movie> movies)
        {
            using (var db = new SpiderDbContext())
            {
                var fromUrls = movies.Select(m => m.FromUrl).ToList();
                var dbMovies = db.Movie.Where(m => fromUrls.Contains(m.FromUrl)).ToList();

                dbMovies.ForEach(m =>
                {
                    var movie = movies.Where(mv => mv.FromUrl == m.FromUrl).First();

                    m.CnName = movie.CnName;
                    m.Region = movie.Region;
                    m.MediaType = movie.MediaType;

                    m.JobStatus = m.JobStatus == JobStatusEnum.SyncDone ? JobStatusEnum.ListDone : m.JobStatus; // 同步完成后, 修改状态, 否则保持原状态

                    m.UpdateTime = DateTime.Now;
                });

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 通过URL取Movie, 只返回 FromUrl/MediaType/IsDone
        /// </summary>
        /// <param name="fromUrls"></param>
        /// <returns></returns>
        public List<MovieModel> GetMoviesByFromUrls(List<string> fromUrls)
        {
            using (var db = new SpiderDbContext())
            {
                var movies = db.Movie.Where(m => fromUrls.Contains(m.FromUrl))
                    .Select(m => new MovieModel
                    {
                        FromUrl = m.FromUrl,
                        MediaType = m.MediaType
                    }).ToList();

                return movies;
            }
        }

        #region 取概要抓取完成的

        /// <summary>
        /// 取概要抓取完成的总数, 分页用
        /// </summary>
        public int GetListDoneCount()
        {
            using (var db = new SpiderDbContext())
            {
                return db.Movie.Where(m => !m.IsSyncedByWeb && m.JobStatus == JobStatusEnum.ListDone).Count();
            }
        }

        /// <summary>
        /// 取概要抓取完成的
        /// </summary>
        /// <param name="index">页号(从1开始)</param>
        /// <param name="size">页大小</param>
        /// <returns></returns>
        public List<MovieModel> GetListDoneMovies(int index, int size)
        {
            using (var db = new SpiderDbContext())
            {
                var skip = (index - 1) * size;
                var movies = db.Movie.Where(m => !m.IsSyncedByWeb && m.JobStatus == JobStatusEnum.ListDone)
                    .Select(m => new MovieModel
                    {
                        MovieId = m.MovieId,
                        FromUrl = m.FromUrl,
                        CreateTime = m.CreateTime
                    })
                    .OrderBy(m => m.MovieId)
                    .Skip(skip).Take(size)
                    .ToList();

                return movies;
            }
        }
        #endregion

        #region 未同步完成
        /// <summary>
        /// 取未同步完成的总数, 分页用
        /// </summary>
        public int GetNotSyncCount()
        {
            using (var db = new SpiderDbContext())
            {
                return db.Movie.Where(m => m.JobStatus == JobStatusEnum.DetailDone && !m.IsSyncedByWeb).Count();
            }
        }

        /// <summary>
        /// 取未同步完成的
        /// </summary>
        public List<Movie> GetNotSyncMovies(int index, int size)
        {
            using (var db = new SpiderDbContext())
            {
                var skip = (index - 1) * size;
                var movies = db.Movie.Where(m => m.JobStatus == JobStatusEnum.DetailDone && !m.IsSyncedByWeb).OrderBy(m => m.MovieId).Skip(skip).Take(size).ToList();

                return movies;
            }
        }

        /// <summary>
        /// 更新同步完成标志
        /// </summary>
        public void UpdateSyncDone(List<int> movieIds)
        {
            using (var db = new SpiderDbContext())
            {
                var movies = db.Movie.Where(m => movieIds.Contains(m.MovieId)).ToList();
                movies.ForEach(m => m.JobStatus = JobStatusEnum.SyncDone);

                db.SaveChanges();
            }
        }

        #endregion
    }
}

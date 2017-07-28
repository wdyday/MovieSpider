using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieSpider.Data.Entities;
using MovieSpider.Core.Pager;
using MovieSpider.Data;

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

        public void UpdateDoneMovie(Movie movie)
        {
            using (var db = new SpiderDbContext())
            {
                var dbMovie = db.Movie.Where(m => m.MovieId == movie.MovieId).FirstOrDefault();
                if (dbMovie != null)
                {
                    dbMovie.CreateTime = movie.CreateTime;
                    dbMovie.Detail = movie.Detail;
                    dbMovie.Summary = movie.Summary;
                    dbMovie.OtherCnNames = movie.OtherCnNames;
                    dbMovie.PremiereDateMulti = movie.PremiereDateMulti;
                    dbMovie.PremiereDate = movie.PremiereDate;
                    dbMovie.IsDone = true;

                    db.SaveChanges();
                }
            }
        }

        public void AddMovies(List<Movie> movies)
        {
            using (var db = new SpiderDbContext())
            {
                movies.ForEach(m =>
                {
                    m.IsDone = false;
                    m.IsSyncDone = false;
                    m.CreateTime = DateTime.Now;
                });
                db.Movie.AddRange(movies);
                db.SaveChanges();
            }
        }

        public List<Movie> GetMoviesByFromUrls(List<string> fromUrls)
        {
            using (var db = new SpiderDbContext())
            {
                var movies = db.Movie.Where(m => fromUrls.Contains(m.FromUrl)).ToList();

                return movies;
            }
        }

        #region 取未抓取完成的

        /// <summary>
        /// 取未抓取完成的总数, 分页用
        /// </summary>
        public int GetNotDoneCount()
        {
            using (var db = new SpiderDbContext())
            {
                return db.Movie.Where(m => !m.IsDone).Count();
            }
        }

        /// <summary>
        /// 取未抓取完成的
        /// </summary>
        /// <param name="index">页号(从1开始)</param>
        /// <param name="size">页大小</param>
        /// <returns></returns>
        public List<Movie> GetNotDoneMovies(int index, int size)
        {
            using (var db = new SpiderDbContext())
            {
                var skip = (index - 1) * size;
                var movies = db.Movie.Where(m => !m.IsDone).OrderBy(m => m.MovieId).Skip(skip).Take(size).ToList();

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
                return db.Movie.Where(m => m.IsDone && !m.IsSyncDone).Count();
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
                var movies = db.Movie.Where(m => m.IsDone && !m.IsSyncDone).OrderBy(m => m.MovieId).Skip(skip).Take(size).ToList();

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
                movies.ForEach(m => m.IsSyncDone = true);

                db.SaveChanges();
            }
        }

        #endregion
    }
}

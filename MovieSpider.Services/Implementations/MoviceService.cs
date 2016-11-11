using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieSpider.Data.Entities;
using MovieSpider.Core.Pager;

namespace MovieSpider.Services
{
    public class MoviceService : BaseService, IMoviceService
    {
        public Movie Get(int id)
        {
            return db.Movie.Where(m => m.MovieId == id).FirstOrDefault();
        }

        public void UpdateDoneMovie(Movie movie)
        {
            var dbMovie = db.Movie.Where(m => m.MovieId == movie.MovieId).FirstOrDefault();
            if (dbMovie != null)
            {
                dbMovie.CreateTime = movie.CreateTime;
                dbMovie.Detail = movie.Detail;
                dbMovie.OtherCnNames = movie.OtherCnNames;
                dbMovie.PremiereDateMulti = movie.PremiereDateMulti;
                dbMovie.PremiereDate = movie.PremiereDate;
                dbMovie.IsDone = true;

                db.SaveChanges();
            }
        }

        public void AddMovies(List<Movie> movies)
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

        public List<Movie> GetMoviesByFromUrls(List<string> fromUrls)
        {
            var movies = db.Movie.Where(m => fromUrls.Contains(m.FromUrl)).ToList();

            return movies;
        }

        /// <summary>
        /// 取未抓取完成的总数, 分页用
        /// </summary>
        public int GetNotDoneCount()
        {
            return db.Movie.Where(m => !m.IsDone).Count();
        }

        /// <summary>
        /// 取未抓取完成的
        /// </summary>
        public List<Movie> GetTopNotDoneMovies(int top)
        {
            var movies = db.Movie.Where(m => !m.IsDone).OrderBy(m => m.MovieId).Take(top).ToList();

            return movies;
        }

        /// <summary>
        /// 取未同步完成的
        /// </summary>
        public List<Movie> GetTopNotSyncMovies(int top)
        {
            var movies = db.Movie.Where(m => m.IsDone && !m.IsSyncDone).OrderBy(m => m.MovieId).Take(top).ToList();

            return movies;
        }

        /// <summary>
        /// 更新同步完成标志
        /// </summary>
        public void UpdateSyncDone(List<int> movieIds)
        {
            var movies = db.Movie.Where(m => movieIds.Contains(m.MovieId)).ToList();
            movies.ForEach(m => m.IsSyncDone = true);

            db.SaveChanges();
        }
    }
}

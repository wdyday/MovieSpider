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
            //db.Movie.ToList();

            return new Movie
            {
                MovieId = 1,
                CnName = "冰河世纪",
                EnName = ""
            };
        }

        public void UpdateMovie(Movie movie)
        {
            var dbMovie = db.Movie.Where(m => m.MovieId == movie.MovieId).FirstOrDefault();
            if (dbMovie != null)
            {
                dbMovie.CreateDate = movie.CreateDate;
                dbMovie.Detail = movie.Detail;
                dbMovie.OtherCnNames = movie.OtherCnNames;
                dbMovie.PremiereDateMulti = movie.PremiereDateMulti;
                dbMovie.PremiereDate = movie.PremiereDate;

                db.SaveChanges();
            }
        }

        public void AddMovies(List<Movie> movies)
        {
            movies.ForEach(m =>
            {
                m.IsDone = false;
                m.CreateDate = DateTime.Now;
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
        /// <returns></returns>
        public int GetNotDoneCount()
        {
            return db.Movie.Where(m => !m.IsDone).Count();
        }

        public PageResult<Movie> GetMovies(int pageIndex, int pageSize)
        {
            var movies = db.Movie.OrderBy(m => m.MovieId).ToPageResult(pageIndex, pageSize);

            return movies;
        }
    }
}

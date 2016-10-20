using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieSpider.Data.Entities;

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
    }
}

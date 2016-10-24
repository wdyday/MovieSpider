using MovieSpider.Core.Pager;
using MovieSpider.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Services
{
    public interface IMoviceService
    {
        Movie Get(int id);

        void UpdateDoneMovie(Movie movie);

        void AddMovies(List<Movie> movies);

        List<Movie> GetMoviesByFromUrls(List<string> fromUrls);

        int GetNotDoneCount();

        List<Movie> GetTopNotDoneMovies(int top);
    }
}

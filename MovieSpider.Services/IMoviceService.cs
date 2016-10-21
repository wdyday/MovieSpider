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

        void UpdateMovie(Movie movie);

        void AddMovies(List<Movie> movies);

        List<Movie> GetMoviesByFromUrls(List<string> fromUrls);

        int GetNotDoneCount();

        PageResult<Movie> GetMovies(int pageIndex, int pageSize);
    }
}

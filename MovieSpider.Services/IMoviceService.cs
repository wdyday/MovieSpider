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

        /// <summary>
        /// 未抓取详情的数据
        /// </summary>
        /// <param name="top">条数</param>
        List<Movie> GetTopNotDoneMovies(int top);

        /// <summary>
        /// 未同步到api的数据
        /// </summary>
        /// <param name="top">条数</param>
        List<Movie> GetTopNotSyncMovies(int top);

        /// <summary>
        /// 更新同步完成标志
        /// </summary>
        void UpdateSyncDone(List<int> movieIds);
    }
}

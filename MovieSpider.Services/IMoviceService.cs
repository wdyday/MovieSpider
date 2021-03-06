﻿using MovieSpider.Core.Pager;
using MovieSpider.Data.Entities;
using MovieSpider.Data.Models;
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

        void AddMovies(List<Movie> movies);

        void UpdateMovies(List<Movie> movies);

        /// <summary>
        /// 通过URL取Movie, 只返回 FromUrl/MediaType/IsDone
        /// </summary>
        /// <param name="fromUrls"></param>
        /// <returns></returns>
        List<MovieModel> GetMoviesByFromUrls(List<string> fromUrls);

        /// <summary>
        /// 取未抓取完成的总数, 分页用
        /// </summary>
        int GetListDoneCount();

        /// <summary>
        /// 未抓取详情的数据
        /// </summary>
        /// <param name="index">页号(从1开始)</param>
        /// <param name="size">页大小</param>
        List<MovieModel> GetListDoneMovies(int index, int size);

        /// <summary>
        /// 更新 详细信息
        /// </summary>
        /// <param name="movie"></param>
        void UpdateMovieDetailDone(MovieModel movie);

        /// <summary>
        /// 网站后台修改的数据同步给 本地抓取库
        /// </summary>
        /// <param name="posts"></param>
        void UpdateMovieByWeb(List<PostModel> posts);

        /// <summary>
        /// 取未抓取完成的总数, 分页用
        /// </summary>
        int GetNotSyncCount();

        /// <summary>
        /// 未同步到api的数据
        /// </summary>
        /// <param name="index">页号(从1开始)</param>
        /// <param name="size">页大小</param>
        List<Movie> GetNotSyncMovies(int index, int size);

        /// <summary>
        /// 更新同步完成标志
        /// </summary>
        void UpdateSyncDone(List<int> movieIds);
    }
}

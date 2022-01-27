using BLL.Dtos;
using BLL.Dtos.News;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface INewsService
    {
        /// <summary>
        /// Create news
        /// </summary>
        /// <param name="newsRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendNewsResponse>> CreateNews(NewsRequest newsRequest);


        /// <summary>
        /// //Get News by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendNewsResponse>> GetNewsById(string id);


        /// <summary>
        /// Get news by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendNewsResponse>>> GetNewsByReleaseDate(DateTime date);


        /// <summary>
        /// get news by apartmentid
        /// </summary>
        /// <param name="aparmentId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendNewsResponse>>> GetNewsByAparmentId(string apartmentId);


        /// <summary>
        /// update news
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsUpdateRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendNewsResponse>> UpdateNewsById(string id, NewsUpdateRequest newsUpdateRequest);


        /// <summary>
        /// delete news
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendNewsResponse>> DeleteNewsById(string id);


        /// <summary>
        /// Get News By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendNewsResponse>>> GetNewsByStatus(int status);


        /// <summary>
        /// Get All News
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendNewsResponse>>> GetAllNews();
    }
}

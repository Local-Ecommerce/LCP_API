using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.News;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private readonly IFirebaseService _firebaseService;
        private const string PREFIX = "NS_";
        private const string TYPE = "News";

        public NewsService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IFirebaseService firebaseService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _firebaseService = firebaseService;
        }

        /// <summary>
        /// Create news
        /// </summary>
        /// <param name="newsRequest"></param>
        /// <returns></returns>
        public async Task<NewsResponse> CreateNews(NewsRequest newsRequest)
        {
            News news = _mapper.Map<News>(newsRequest);

            try
            {
                news.NewsId = _utilService.CreateId(PREFIX);
                news.ReleaseDate = _utilService.CurrentTimeInVietnam();
                news.Status = (int)NewsStatus.ACTIVE_NEWS;
                news.Image = _firebaseService
                        .UploadFilesToFirebase(newsRequest.Image, TYPE, news.NewsId, "Image", 0)
                        .Result;

                _unitOfWork.News.Add(news);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.CreateNews()]: " + e.Message);

                throw;
            }

            return _mapper.Map<NewsResponse>(news);
        }


        /// <summary>
        ///  Update News By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsUpdateRequest"></param>
        /// <returns></returns>
        public async Task<NewsResponse> UpdateNewsById(string id, NewsUpdateRequest newsUpdateRequest)
        {
            News news;
            try
            {
                news = await _unitOfWork.News.FindAsync(local => local.NewsId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.UpdateNewsById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(News), id);
            }

            //Update News to DB
            try
            {
                string imageUrl = news.Image;

                //update image
                if (newsUpdateRequest.Image != null && newsUpdateRequest.Image.Length > 0)
                {
                    //get the order of the last photo
                    int order = !string.IsNullOrEmpty(news.Image) ? _utilService.LastImageNumber("Image", news.Image) : 0;

                    //upload new image & remove image
                    foreach (var image in newsUpdateRequest.Image)
                    {
                        if (image.Contains("https://firebasestorage.googleapis.com/"))
                            imageUrl = imageUrl.Replace(image + "|", "");
                        else
                            imageUrl += _firebaseService
                                .UploadFilesToFirebase(new string[] { image }, TYPE, news.NewsId, "Image", order).Result;
                    }
                }

                news = _mapper.Map(newsUpdateRequest, news);
                news.Image = imageUrl;

                _unitOfWork.News.Update(news);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.UpdateNewsById()]: " + e.Message);

                throw;
            }

            return _mapper.Map<NewsResponse>(news);
        }


        /// <summary>
        /// Delete News
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteNewsById(string id)
        {
            //Check id
            News news;
            try
            {
                news = await _unitOfWork.News.FindAsync(local => local.NewsId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.DeleteNewsById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(News), id);
            }

            //Delete News
            try
            {
                news.Status = (int)NewsStatus.INACTIVE_NEWS;

                _unitOfWork.News.Update(news);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.DeleteNewsById()]: " + e.Message);

                throw;
            }
        }


        /// <summary>
        /// Get News
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="search"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<object> GetNews(
            string id, string apartmentId, string type,
            DateTime date, string search, int?[] status,
            int? limit, int? page,
            string[] sort, string[] include)
        {
            PagingModel<News> news;

            //sort
            List<string> sortProperty = null;
            if (!_utilService.IsNullOrEmpty(sort))
            {
                sortProperty = new();
                foreach (var param in sort)
                    if (!string.IsNullOrEmpty(param))
                    {
                        string direction = sort[0].ToString().Equals("+") ? "" : " descending";
                        sortProperty.Add(_utilService.UpperCaseFirstLetter(param[1..]) + direction);
                    }
            }

            //include
            for (int i = 0; i < include.Length; i++)
            {
                include[i] = !string.IsNullOrEmpty(include[i]) ? _utilService.UpperCaseFirstLetter(include[i]) : null;
            }

            try
            {
                news = await _unitOfWork.News
                    .GetNews(id, apartmentId, type, date, search, status, limit, page, sortProperty, include);
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.GetNews()]" + e.Message);
                throw;
            }

            return new PagingModel<ExtendNewsResponse>
            {
                List = _mapper.Map<List<ExtendNewsResponse>>(news.List),
                Page = news.Page,
                LastPage = news.LastPage,
                Total = news.Total,
            };
        }
    }
}

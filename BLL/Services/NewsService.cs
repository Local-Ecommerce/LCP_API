using AutoMapper;
using BLL.Constants;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.News;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using BLL.Dtos.MarketManager;
using BLL.Dtos.Apartment;

namespace BLL.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "NS_";

        public NewsService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
        }

        /// <summary>
        /// Create news
        /// </summary>
        /// <param name="newsRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<NewsResponse>> CreateNews(NewsRequest newsRequest)
        {
            News news = _mapper.Map<News>(newsRequest);

            try
            {
                news.NewsId = _utilService.CreateId(PREFIX);
                news.ReleaseDate = DateTime.Now;
                news.Status = (int)NewsStatus.ACTIVE_NEWS;

                _unitOfWork.Repository<News>().Add(news);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.CreateNews()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<NewsResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }
            //Create Response
            NewsResponse newsResponse = _mapper.Map<NewsResponse>(news);

            return new BaseResponse<NewsResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponse
            };
        }

        /// <summary>
        /// Get News By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<NewsResponse>> GetNewsById(string id)
        {
            NewsResponse newsReponse;

            //Get News from DB

            try
            {
                News news = await _unitOfWork.Repository<News>().FindAsync(local => local.NewsId.Equals(id));

                newsReponse = _mapper.Map<NewsResponse>(news);
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.GetNewsById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                    ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<NewsResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsReponse
            };
        }

        /// <summary>
        /// Get News By Release Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<NewsResponse>>> GetNewsByReleaseDate(DateTime date)
        {
            List<NewsResponse> newsResponses;

            //Get News from DB

            try
            {
                List<News> news = await _unitOfWork.Repository<News>().FindListAsync(news => news.ReleaseDate.Value.Date == date.Date);

                newsResponses = _mapper.Map<List<NewsResponse>>(news);
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.GetNewsByReleasedDate()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                    ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<List<NewsResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponses
            };
        }

        /// <summary>
        /// GetNewsByApartmentId
        /// </summary>
        /// <param name="apatrmentId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<NewsResponse>>> GetNewsByAparmentId(string apatrmentId)
        {
            List<NewsResponse> newsResponses;

            //Get ApartmentId from DB

            try
            {
                List<News> news = await _unitOfWork.Repository<News>().FindListAsync(news => news.ApartmentId.Equals(apatrmentId));

                newsResponses = _mapper.Map<List<NewsResponse>>(news);
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.GetNewsByAparmentId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                    ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<List<NewsResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponses
            };
        }

        /// <summary>
        /// GetNewsByMarketManagerId
        /// </summary>
        /// <param name="marketmanagerId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<NewsResponse>>> GetNewsByMarketManagerId(string marketmanagerId)
        {
            List<NewsResponse> newsResponses;

            //Get News from DB

            try
            {
                List<News> news = await _unitOfWork.Repository<News>()
                    .FindListAsync(news => news.MarketManagerId.Equals(marketmanagerId));

                newsResponses = _mapper.Map<List<NewsResponse>>(news);
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.GetNewsByMarketManagerId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                    ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<List<NewsResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponses
            };
        }

        /// <summary>
        ///  Update News By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<NewsResponse>> UpdateNewsById(string id, NewsRequest newsRequest)
        {
            News news;
            try
            {
                news = await _unitOfWork.Repository<News>().FindAsync(local => local.NewsId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.UpdateNewsById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                    ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Update News to DB
            try
            {
                news = _mapper.Map(newsRequest, news);

                _unitOfWork.Repository<News>().Update(news);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.UpdateNewsById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            NewsResponse newsResponse = _mapper.Map<NewsResponse>(news);

            return new BaseResponse<NewsResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponse
            };
        }

        /// <summary>
        /// Delete News
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<NewsResponse>> DeleteNewsById(string id)
        {
            //Check id
            News news;
            try
            {
                news = await _unitOfWork.Repository<News>().FindAsync(local => local.NewsId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.DeleteNewsById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                    ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Delete News
            try
            {
                news.Status = (int)NewsStatus.INACTIVE_NEWS;

                _unitOfWork.Repository<News>().Update(news);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.DeleteNewsById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            NewsResponse newsResponse = _mapper.Map<NewsResponse>(news);

            return new BaseResponse<NewsResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponse
            };
        }


        /// <summary>
        /// Get News By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<NewsResponse>>> GetNewsByStatus(int status)
        {
            List<NewsResponse> newsList = null;

            //get News from database
            try
            {
                newsList = _mapper.Map<List<NewsResponse>>(
                    await _unitOfWork.Repository<News>()
                                     .FindListAsync(mar => mar.Status == status));
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.GetNewsByStatus()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<NewsResponse>
                    {
                        ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                        ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<NewsResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsList
            };
        }

        /// <summary>
        /// Get All News
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<List<NewsResponse>>> GetAllNews()
        {
            List<NewsResponse> newsList = null;

            try
            {
                await using var context = new LoichDBContext();
                newsList = (from news in context.News
                            join mm in context.MarketManagers
                            on news.MarketManagerId equals mm.MarketManagerId
                            join ap in context.Apartments
                            on news.ApartmentId equals ap.ApartmentId
                            orderby news.ReleaseDate descending
                            select new NewsResponse
                            {
                                ApartmentId = news.ApartmentId,
                                MarketManagerId = news.MarketManagerId,
                                NewsId = news.NewsId,
                                ReleaseDate = news.ReleaseDate,
                                Status = news.Status,
                                Text = news.Text,
                                Title = news.Title,
                                MarketManager = new MarketManagerResponse
                                {
                                    MarketManagerId = mm.MarketManagerId,
                                    Status = mm.Status,
                                    MarketManagerName = mm.MarketManagerName,
                                    AccountId = mm.AccountId,
                                    PhoneNumber = mm.PhoneNumber
                                },
                                Apartment = new ApartmentResponse
                                {
                                    ApartmentId = ap.ApartmentId,
                                    Address = ap.Address,
                                    Lat = ap.Lat,
                                    Long = ap.Long,
                                    Status = ap.Status
                                }
                            }).ToList();
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.GetAllNews()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<NewsResponse>
                    {
                        ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                        ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<NewsResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsList
            };
        }
    }
}

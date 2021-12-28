using AutoMapper;
using BLL.Dtos;
using BLL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.SystemCategory;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class SystemCategoryService : ISystemCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string CACHE_KEY = "SystemCategory";

        public SystemCategoryService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _utilService = utilService;
        }

        /// <summary>
        /// Create System Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<SystemCategoryResponse>> CreateSystemCategory(SystemCategoryRequest request)
        {
            //biz rule

            //store systemCategory to database
            SystemCategory systemCategory = _mapper.Map<SystemCategory>(request);
            try
            {
                systemCategory.SystemCategoryId = _utilService.Create16Alphanumeric();
                systemCategory.Status = (int)SystemCategoryStatus.ACTIVE_SYSTEM_CATEGORY;
                systemCategory.ApproveBy = "";

                _unitOfWork.Repository<SystemCategory>().Add(systemCategory);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.CreateSystemCategory()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<SystemCategoryResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //create response
            SystemCategoryResponse systemCategoryResponse = _mapper.Map<SystemCategoryResponse>(systemCategory);

            return new BaseResponse<SystemCategoryResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = systemCategoryResponse
            };
        }


        /// <summary>
        /// Delete System Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<SystemCategoryResponse>> DeleteSystemCategory(string id)
        {
            //biz rule

            //validate id
            SystemCategory systemCategory;
            try
            {
                systemCategory = await _unitOfWork.Repository<SystemCategory>()
                                           .FindAsync(p => p.SystemCategoryId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.DeleteSystemCategory()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<SystemCategory>
                    {
                        ResultCode = (int)SystemCategoryStatus.SYSTEM_CATEGORY_NOT_FOUND,
                        ResultMessage = SystemCategoryStatus.SYSTEM_CATEGORY_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //delete systemCategory
            try
            {
                systemCategory.Status = (int)SystemCategoryStatus.DELETED_SYSTEM_CATEGORY;
                systemCategory.ApproveBy = "";

                _unitOfWork.Repository<SystemCategory>().Update(systemCategory);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.DeleteSystemCategory()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<SystemCategory>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //create response
            SystemCategoryResponse systemCategoryResponse = _mapper.Map<SystemCategoryResponse>(systemCategory);

            return new BaseResponse<SystemCategoryResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = default
            };
        }


        /// <summary>
        /// Get All System Category
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<SystemCategoryResponse>>> GetAllSystemCategory()
        {
            List<SystemCategoryResponse> systemCategoryList = null;

                //get systemCategory from database
                try
                {
                    systemCategoryList = _mapper.Map<List<SystemCategoryResponse>>(
                        await _unitOfWork.Repository<SystemCategory>()
                                         .FindListAsync(sc => sc.SystemCategoryId != null));
                }
                catch (Exception e)
                {
                    _logger.Error("[SystemCategoryService.GetAllSystemCategory()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<SystemCategoryResponse>
                        {
                            ResultCode = (int)SystemCategoryStatus.SYSTEM_CATEGORY_NOT_FOUND,
                            ResultMessage = SystemCategoryStatus.SYSTEM_CATEGORY_NOT_FOUND.ToString(),
                            Data = default
                        });
                }

            return new BaseResponse<List<SystemCategoryResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = systemCategoryList
            };
        }


        /// <summary>
        /// Update System Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<SystemCategoryResponse>> UpdateSystemCategory(string id, SystemCategoryRequest request)
        {
            //biz rule

            //validate id
            SystemCategory systemCategory;
            try
            {
                systemCategory = await _unitOfWork.Repository<SystemCategory>()
                                           .FindAsync(p => p.SystemCategoryId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.UpdateSystemCategory()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<SystemCategory>
                    {
                        ResultCode = (int)SystemCategoryStatus.SYSTEM_CATEGORY_NOT_FOUND,
                        ResultMessage = SystemCategoryStatus.SYSTEM_CATEGORY_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //update data
            try
            {
                systemCategory = _mapper.Map(request, systemCategory);
                systemCategory.ApproveBy = "";

                _unitOfWork.Repository<SystemCategory>().Update(systemCategory);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.UpdateSystemCategory()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<SystemCategory>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //create response
            SystemCategoryResponse systemCategoryResponse = _mapper.Map<SystemCategoryResponse>(systemCategory);

            return new BaseResponse<SystemCategoryResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = systemCategoryResponse
            };
        }


        /// <summary>
        /// Get System Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<SystemCategoryResponse>> GetSystemCategoryById(string id)
        {
            SystemCategoryResponse systemCategoryResponse;

                //get systemCategory from database
                try
                {
                    SystemCategory systemCategory = await _unitOfWork.Repository<SystemCategory>()
                                                       .FindAsync(p => p.SystemCategoryId.Equals(id));
                    systemCategoryResponse = _mapper.Map<SystemCategoryResponse>(systemCategory);

                }
                catch (Exception e)
                {
                    _logger.Error("[SystemCategoryService.GetSystemCategoryById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<SystemCategoryResponse>
                        {
                            ResultCode = (int)SystemCategoryStatus.SYSTEM_CATEGORY_NOT_FOUND,
                            ResultMessage = SystemCategoryStatus.SYSTEM_CATEGORY_NOT_FOUND.ToString(),
                            Data = default
                        });
                }

            return new BaseResponse<SystemCategoryResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = systemCategoryResponse
            };
        }
    }
}

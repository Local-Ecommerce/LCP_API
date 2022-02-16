using AutoMapper;
using BLL.Dtos;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.SystemCategory;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BLL.Services
{
    public class SystemCategoryService : ISystemCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "SC_";
        private const string CACHE_KEY = "System Category";

        public SystemCategoryService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
        }

        /// <summary>
        /// Create System Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<SystemCategoryResponse> CreateSystemCategory(SystemCategoryRequest request)
        {
            //biz rule

            //store systemCategory to database
            SystemCategory systemCategory = _mapper.Map<SystemCategory>(request);
            try
            {
                systemCategory.SystemCategoryId = _utilService.CreateId(PREFIX);
                systemCategory.Status = (int)SystemCategoryStatus.ACTIVE_SYSTEM_CATEGORY;
                systemCategory.ApproveBy = "";

                int? level;

                if (systemCategory.BelongTo != null)
                {
                    int? parentLevel = (await _unitOfWork.SystemCategories.FindAsync(sc =>
                                            sc.SystemCategoryId.Equals(systemCategory.BelongTo))).CategoryLevel;

                    if (parentLevel == (int?)CategoryLevel.THREE)
                    {
                        _logger.Error("[SystemCategoryService.CreateSystemCategory()]: Max level has been reached.");

                        throw new HttpStatusException(HttpStatusCode.OK,
                            new ApiResponseFailed<SystemCategoryResponse>
                            {
                                ResultCode = (int)SystemCategoryStatus.MAXED_OUT_LEVEL,
                                ResultMessage = SystemCategoryStatus.MAXED_OUT_LEVEL.ToString()
                            });
                    }
                    else
                        level = parentLevel + 1;
                }
                else
                    level = (int)CategoryLevel.ONE;

                systemCategory.CategoryLevel = level;

                _unitOfWork.SystemCategories.Add(systemCategory);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (HttpStatusException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.CreateSystemCategory()]: " + e.Message);

                throw;
            }

            //create response
            SystemCategoryResponse systemCategoryResponse = _mapper.Map<SystemCategoryResponse>(systemCategory);

            if (systemCategoryResponse.CategoryLevel != (int)CategoryLevel.THREE)
                systemCategoryResponse.InverseBelongToNavigation = new Collection<SystemCategoryResponse>();

            return systemCategoryResponse;
        }


        /// <summary>
        /// Delete System Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<SystemCategoryResponse> DeleteSystemCategory(string id)
        {
            //biz rule

            //validate id
            SystemCategory systemCategory;
            try
            {
                systemCategory = await _unitOfWork.SystemCategories.FindAsync(p => p.SystemCategoryId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.DeleteSystemCategory()]" + e.Message);

                throw new EntityNotFoundException(typeof(SystemCategory), id);
            }

            //delete systemCategory
            try
            {
                systemCategory.Status = (int)SystemCategoryStatus.DELETED_SYSTEM_CATEGORY;
                systemCategory.ApproveBy = "";

                _unitOfWork.SystemCategories.Update(systemCategory);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.DeleteSystemCategory()]" + e.Message);

                throw;
            }

            return _mapper.Map<SystemCategoryResponse>(systemCategory);
        }


        /// <summary>
        /// Get All System Category
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemCategoryResponse>> GetAllSystemCategory()
        {
            List<SystemCategory> systemCategories;

            //get systemCategory from database
            try
            {
                systemCategories = await _unitOfWork.SystemCategories.GetAllSystemCategoryIncludeInverseBelongTo();
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.GetAllSystemCategory()]: " + e.Message);

                throw new EntityNotFoundException(typeof(SystemCategory), "all");
            }

            return _mapper.Map<List<SystemCategoryResponse>>(systemCategories);
        }


        /// <summary>
        /// Update System Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<SystemCategoryResponse> UpdateSystemCategory(string id,
            SystemCategoryUpdateRequest request)
        {
            //biz rule

            //validate id
            SystemCategory systemCategory;
            try
            {
                systemCategory = await _unitOfWork.SystemCategories
                                           .FindAsync(p => p.SystemCategoryId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.UpdateSystemCategory()]" + e.Message);

                throw new EntityNotFoundException(typeof(SystemCategory), id);
            }

            //update data
            try
            {
                systemCategory = _mapper.Map(request, systemCategory);
                systemCategory.ApproveBy = "";

                _unitOfWork.SystemCategories.Update(systemCategory);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.UpdateSystemCategory()]" + e.Message);

                throw;
            }

            return _mapper.Map<SystemCategoryResponse>(systemCategory);
        }


        /// <summary>
        /// Get System Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<SystemCategoryResponse> GetSystemCategoryById(string id)
        {
            SystemCategory systemCategory;
            //get systemCategory from database
            try
            {
                systemCategory = await _unitOfWork.SystemCategories
                                            .GetSystemCategoryByIdIncludeInverseBelongTo(id);

            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.GetSystemCategoryById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(SystemCategory), id);
            }

            return _mapper.Map<SystemCategoryResponse>(systemCategory);
        }


        /// <summary>
        /// Get System Category And One Level Down Inverse Belong To By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<SystemCategoryResponse> GetSystemCategoryAndOneLevelDownInverseBelongToById(string id)
        {
            SystemCategory systemCategory;
            //get systemCategory from database
            try
            {
                systemCategory = await _unitOfWork.SystemCategories
                                            .GetSystemCategoryByIdIncludeOneLevelDownInverseBelongTo(id);

            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.GetSystemCategoryById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(SystemCategory), id);
            }

            return _mapper.Map<SystemCategoryResponse>(systemCategory);
        }


        /// <summary>
        /// Get System Categories By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<SystemCategoryResponse>> GetSystemCategoriesByStatus(int status)
        {
            List<SystemCategoryResponse> systemCategoryList = null;

            //get SystemCategory from database
            try
            {
                systemCategoryList = _mapper.Map<List<SystemCategoryResponse>>(
                    await _unitOfWork.SystemCategories
                                     .FindListAsync(SystemCategory => SystemCategory.Status == status));
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.GetSystemCategorysByStatus()]: " + e.Message);

                throw new EntityNotFoundException(typeof(SystemCategory), status);
            }

            return systemCategoryList;
        }


        /// <summary>
        /// Get System Categories For Auto Complete
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<SystemCategoryForAutoCompleteResponse>> GetSystemCategoriesForAutoComplete()
        {
            List<SystemCategoryForAutoCompleteResponse> systemCategoryList = null;

            //get SystemCategory from database
            try
            {
                systemCategoryList = _mapper.Map<List<SystemCategoryForAutoCompleteResponse>>(
                    await _unitOfWork.SystemCategories.GetAllLevelOneAndTwoSystemCategory());
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.GetSystemCategoriesForAutoComplete()]: " + e.Message);

                throw new EntityNotFoundException(typeof(SystemCategory), "autoComplete");
            }

            return systemCategoryList;
        }
    }
}

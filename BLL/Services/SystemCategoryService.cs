using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.SystemCategory;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
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
        public async Task<ParentSystemCategoryResponse> CreateSystemCategory(SystemCategoryRequest request)
        {
            //biz rule

            //store systemCategory to database
            SystemCategory systemCategory = _mapper.Map<SystemCategory>(request);
            try
            {
                systemCategory.SystemCategoryId = _utilService.CreateId(PREFIX);
                systemCategory.Status = (int)SystemCategoryStatus.ACTIVE_SYSTEM_CATEGORY;

                int? level;

                if (systemCategory.BelongTo != null)
                {
                    int? parentLevel = (await _unitOfWork.SystemCategories.FindAsync(sc =>
                                            sc.SystemCategoryId.Equals(systemCategory.BelongTo))).CategoryLevel;

                    if (parentLevel == (int?)CategoryLevel.THREE)
                        throw new BusinessException(SystemCategoryStatus.MAXED_OUT_LEVEL.ToString(), (int)SystemCategoryStatus.MAXED_OUT_LEVEL);
                    else
                        level = parentLevel + 1;
                }
                else
                    level = (int)CategoryLevel.ONE;

                systemCategory.CategoryLevel = level;

                _unitOfWork.SystemCategories.Add(systemCategory);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.CreateSystemCategory()]: " + e.Message);
                throw;
            }

            //create response
            ParentSystemCategoryResponse systemCategoryResponse = _mapper.Map<ParentSystemCategoryResponse>(systemCategory);

            if (systemCategoryResponse.CategoryLevel != (int)CategoryLevel.THREE)
                systemCategoryResponse.Children = new Collection<ParentSystemCategoryResponse>();

            return systemCategoryResponse;
        }


        /// <summary>
        /// Delete System Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteSystemCategory(string id)
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

                _unitOfWork.SystemCategories.Update(systemCategory);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.DeleteSystemCategory()]" + e.Message);

                throw;
            }
        }


        /// <summary>
        /// Get System Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="merchantId"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<object> GetSystemCategories(
            string id, string merchantId,
            int?[] status, int? limit,
            int? page, string sort, string include)
        {
            PagingModel<SystemCategory> categories;
            string propertyName = default;
            bool isAsc = false;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            try
            {
                categories = await _unitOfWork.SystemCategories
                    .GetSystemCategory(id, merchantId, status, limit, page, isAsc, propertyName, include);
            }
            catch (Exception e)
            {
                _logger.Error("[SystemCategoryService.GetSystemCategory()]" + e.Message);
                throw;
            }

            if (string.IsNullOrEmpty(include))


                return new PagingModel<ParentSystemCategoryResponse>
                {
                    List = _mapper.Map<List<ParentSystemCategoryResponse>>(categories.List),
                    Page = categories.Page,
                    LastPage = categories.LastPage,
                    Total = categories.Total,
                };

            return new PagingModel<ChildSystemCategoryResponse>
            {
                List = _mapper.Map<List<ChildSystemCategoryResponse>>(categories.List),
                Page = categories.Page,
                LastPage = categories.LastPage,
                Total = categories.Total,
            };

        }


        /// <summary>
        /// Update System Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
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
    }
}

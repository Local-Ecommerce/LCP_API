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
using System.Linq;

namespace BLL.Services
{
    public class SystemCategoryService : ISystemCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private readonly IFirebaseService _firebaseService;
        private const string PREFIX = "SC_";
        private const string TYPE = "Category";

        public SystemCategoryService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IFirebaseService firebaseService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _firebaseService = firebaseService;
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
                systemCategory.CategoryImage = _firebaseService
                        .UploadFileToFirebase(request.CategoryImage, TYPE, systemCategory.SystemCategoryId, "Image")
                        .Result;

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
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<object> GetSystemCategories(
            string id, string merchantId,
            int?[] status, string search, int? limit,
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
                    .GetSystemCategory(id, merchantId, status, search, limit, page, isAsc, propertyName, include);
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
            SystemCategory systemCategory;
            try
            {
                systemCategory = await _unitOfWork.SystemCategories
                                           .FindAsync(p => p.SystemCategoryId.Equals(id));

                int order = !string.IsNullOrEmpty(systemCategory.CategoryImage) ?
                        _utilService.LastImageNumber("Image", systemCategory.CategoryImage) : 0;

                systemCategory = _mapper.Map(request, systemCategory);
                systemCategory.CategoryImage = _firebaseService
                                .UploadFileToFirebase(systemCategory.CategoryImage, TYPE, id, "Image" + (order + 1))
                                .Result;

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
        /// Get System Category Ids By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<string>> GetSystemCategoryIdsById(string id)
        {
            if (id == null)
                return null;

            List<string> categoryIds = new();
            SystemCategory systemCategory = (await _unitOfWork.SystemCategories
                    .GetSystemCategory(id, null, new int?[] { }, null, null, null, false, null, null))
                    .List.FirstOrDefault();

            //if sysCate lv1
            if (systemCategory != null)
            {
                categoryIds.Add(systemCategory.SystemCategoryId);
                if (!_utilService.IsNullOrEmpty(systemCategory.InverseBelongToNavigation))
                {
                    foreach (var scLvDown in systemCategory.InverseBelongToNavigation)
                    {
                        categoryIds.Add(scLvDown.SystemCategoryId);
                        if (!_utilService.IsNullOrEmpty(scLvDown.InverseBelongToNavigation))
                        {
                            foreach (var sc2LvDown in scLvDown.InverseBelongToNavigation)
                            {
                                categoryIds.Add(scLvDown.SystemCategoryId);
                            }
                        }
                    }
                }
            }
            else
            {
                //if sysCate lv2, lv3
                List<SystemCategory> systemCategories = await _unitOfWork.SystemCategories
                    .FindListAsync(sc => sc.SystemCategoryId.Equals(id) || (sc.BelongTo != null && sc.BelongTo.Equals(id)));

                categoryIds = systemCategories.Select(sc => sc.SystemCategoryId).ToList();
            }
            return categoryIds;
        }
    }
}
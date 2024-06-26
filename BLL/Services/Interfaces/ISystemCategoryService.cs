﻿using BLL.Dtos.SystemCategory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ISystemCategoryService
    {
        /// <summary>
        /// Create System Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ParentSystemCategoryResponse> CreateSystemCategory(SystemCategoryRequest request);


        /// <summary>
        /// Update System Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SystemCategoryResponse> UpdateSystemCategory(string id, SystemCategoryUpdateRequest request);


        /// <summary>
        /// Delete System Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteSystemCategory(string id);


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
        Task<object> GetSystemCategories(
            string id, string merchantId,
            int?[] status, string search, int? limit,
            int? page, string sort, string include);


        /// <summary>
        /// Get System Category Ids By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<string>> GetSystemCategoryIdsById(string id);
    }
}

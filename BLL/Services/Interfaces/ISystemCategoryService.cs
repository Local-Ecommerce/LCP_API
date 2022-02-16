using BLL.Dtos.SystemCategory;
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
        Task<SystemCategoryResponse> CreateSystemCategory(SystemCategoryRequest request);


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
        Task<SystemCategoryResponse> DeleteSystemCategory(string id);


        /// <summary>
        /// Get All System Category
        /// </summary>
        /// <returns></returns>
        Task<List<SystemCategoryResponse>> GetAllSystemCategory();


        /// <summary>
        /// Get System Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SystemCategoryResponse> GetSystemCategoryById(string id);


        /// <summary>
        /// Get System Categories By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<List<SystemCategoryResponse>> GetSystemCategoriesByStatus(int status);


        /// <summary>
        /// Get System Categories By Status
        /// </summary>
        /// <returns></returns>
        Task<List<SystemCategoryForAutoCompleteResponse>> GetSystemCategoriesForAutoComplete();


        /// <summary>
        /// Get System Category And One Level Down Inverse Belong To By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SystemCategoryResponse> GetSystemCategoryAndOneLevelDownInverseBelongToById(string id);
    }
}

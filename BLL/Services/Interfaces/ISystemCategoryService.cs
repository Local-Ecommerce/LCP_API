using BLL.Dtos;
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
        Task<BaseResponse<SystemCategoryResponse>> CreateSystemCategory(SystemCategoryRequest request);


        /// <summary>
        /// Update System Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseResponse<SystemCategoryResponse>> UpdateSystemCategory(string id, SystemCategoryRequest request);


        /// <summary>
        /// Delete System Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<SystemCategoryResponse>> DeleteSystemCategory(string id);


        /// <summary>
        /// Get All System Category
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<SystemCategoryResponse>>> GetAllSystemCategory();


        /// <summary>
        /// Get System Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<SystemCategoryResponse>> GetSystemCategoryById(string id);


        /// <summary>
        /// Get System Categories By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<BaseResponse<List<SystemCategoryResponse>>> GetSystemCategoriesByStatus(int status);
        
        
        /// <summary>
        /// Get System Categories By Status
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<SystemCategoryForAutoCompleteResponse>>> GetSystemCategoriesForAutoComplete();
    }
}

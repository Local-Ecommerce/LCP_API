using BLL.Dtos.Menu;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IMenuService
    {
        /// <summary>
        /// Create Menu
        /// </summary>
        /// <param name="residentId"></param>
        /// <param name="menuRequest"></param>
        /// <returns></returns>
        Task<MenuResponse> CreateMenu(string residentId, MenuRequest menuRequest);


        /// <summary>
        /// Create Base Menu
        /// </summary>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        MenuResponse CreateBaseMenu(string merchantStoreId);


        /// <summary>
        /// Get Menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="residentId"></param>
        /// <param name="apartmentId"></param>
        /// <param name="isActive"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<object> GetMenus(
            string id, int?[] status,
            string apartmentId, bool? isActive, int? limit,
            int? page, string sort, string[] include);


        /// <summary>
        /// Update Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuUpdateRequest"></param>
        /// <returns></returns>
        Task<MenuResponse> UpdateMenuById(string id, MenuUpdateRequest menuUpdateRequest);


        /// <summary>
        /// Delete Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteMenuById(string id);
    }
}

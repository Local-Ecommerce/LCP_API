using BLL.Dtos.Menu;
using System;
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
        /// <param name="merchantStoreId"></param>
        /// <param name="search"></param>
        /// <param name="isActive"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<object> GetMenus(
            string id, int?[] status, string residentId,
            string apartmentId, string merchantStoreId, string search, bool? isActive, int? limit,
            int? page, string sort, string[] include, string role);


        /// <summary>
        /// Update Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuUpdateRequest"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<MenuResponse> UpdateMenuById(string id, MenuUpdateRequest menuUpdateRequest, string residentId);


        /// <summary>
        /// Delete Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteMenuById(string id);


        /// <summary>
        /// Get Other Menu Has Same Time
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <param name="repeatDate"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<string> GetOtherMenuHasSameTime(TimeSpan timeStart, TimeSpan timeEnd, string repeatDate, string residentId);
    }
}

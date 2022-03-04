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
        /// Create Default Menu
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        MenuResponse CreateDefaultMenu(string storeName, string merchantStoreId);


        /// <summary>
        /// Get Menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="residentId"></param>
        /// <param name="apartmentId"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<object> GetMenu(
            string id, int?[] status,
            string residentId, string apartmentId, int? limit,
            int? page, string sort, string include);


        /// <summary>
        /// Update Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuRequest"></param>
        /// <returns></returns>
        Task<MenuResponse> UpdateMenuById(string id, MenuRequest menuRequest);


        /// <summary>
        /// Delete Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MenuResponse> DeleteMenuById(string id);
    }
}

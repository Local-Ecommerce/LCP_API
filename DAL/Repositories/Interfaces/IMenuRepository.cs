using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IMenuRepository : IRepository<Menu>
    {
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
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<Menu>> GetMenu(
            string id = default, int?[] status = default, string residentId = default,
            string apartmentId = default, string merchantStoreId = default, string search = default,
            bool? isActive = default, int? limit = default,
            int? queryPage = default, bool? isAsc = default,
            string propertyName = default, string[] include = default);


        /// <summary>
        /// Get Base Menu Id
        /// </summary>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<string> GetBaseMenuId(string residentId);
    }
}
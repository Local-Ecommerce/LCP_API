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
        /// <param name="apartmentId"></param>
        /// <param name="isActive"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<Menu>> GetMenu(
            string id, int?[] status,
            string apartmentId, bool? isActive, int? limit,
            int? queryPage, bool? isAsc,
            string propertyName, string[] include);


        /// <summary>
        /// Get Base Menu Id
        /// </summary>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<string> GetBaseMenuId(string residentId);
    }
}
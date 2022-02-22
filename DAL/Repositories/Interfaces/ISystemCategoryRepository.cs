using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface ISystemCategoryRepository : IRepository<SystemCategory>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        Task<PagingModel<SystemCategory>> GetSystemCategory(
            string id, int? limit,
            int? queryPage, bool isAsc,
            string propertyName);
    }
}
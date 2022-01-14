using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface ISystemCategoryRepository : IRepository<SystemCategory>
    {
        /// <summary>
        /// Get All System Category Include Inverse Belong To
        /// </summary>
        /// <returns></returns>
        Task<List<SystemCategory>> GetAllSystemCategoryIncludeInverseBelongTo();


        /// <summary>
        /// Get All Level One And Two System Category
        /// </summary>
        /// <returns></returns>
        Task<List<SystemCategory>> GetAllLevelOneAndTwoSystemCategory();
    }
}
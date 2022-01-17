using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface INewsRepository : IRepository<News>
    {
        /// <summary>
        /// Get All News Include Resident And Apartment
        /// </summary>
        /// <returns></returns>
        Task<List<News>> GetAllNewsIncludeApartmentAndResident();


        /// <summary>
        /// Get News Include Resident By News Id
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        Task<News> GetNewsIncludeResidentByNewsId(string newsId);
    }
}
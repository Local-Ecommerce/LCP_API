using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface INewsRepository : IRepository<News>
    {
        /// <summary>
        /// Get All News Include Market Manager And Apartment
        /// </summary>
        /// <returns></returns>
        Task<List<News>> GetAllNewsIncludeApartment();
    }
}
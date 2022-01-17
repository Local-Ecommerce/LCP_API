using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        public NewsRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get All News Include Resident And Apartment
        /// </summary>
        /// <returns></returns>
        public async Task<List<News>> GetAllNewsIncludeApartmentAndResident()
        {
            List<News> news = await _context.News
                                .Include(news => news.Apartment)
                                .Include(news => news.Resident)
                                .OrderByDescending(news => news.ReleaseDate)
                                .ToListAsync();

            return news;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        public async Task<News> GetNewsIncludeResidentByNewsId(string newsId)
        {
            News news = await _context.News
                                .Where(news => news.NewsId.Equals(newsId))
                                .Include(news => news.Resident)
                                .OrderByDescending(news => news.ReleaseDate)
                                .FirstOrDefaultAsync();

            return news;
        }
    }
}
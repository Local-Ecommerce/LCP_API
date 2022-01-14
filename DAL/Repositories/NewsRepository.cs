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
        /// Get All News Include Market Manager And Apartment
        /// </summary>
        /// <returns></returns>
        public async Task<List<News>> GetAllNewsIncludeMarketManagerAndApartment()
        {
            List<News> news = await _context.News
                                .Include(news => news.MarketManager)
                                .Include(news => news.Apartment)
                                .OrderByDescending(news => news.ReleaseDate)
                                .ToListAsync();

            return news;
        }
    }
}
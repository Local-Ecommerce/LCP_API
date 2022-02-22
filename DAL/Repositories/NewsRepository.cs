using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System;

namespace DAL.Repositories
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        public NewsRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get News
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="date"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<News>> GetNews(
            string id, string apartmentId, 
            DateTime date, int?[] status, 
            int? limit, int? queryPage, 
            bool isAsc, string propertyName, string[] include)
        {
            IQueryable<News> query = _context.News.Where(news => news.NewsId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(news => news.NewsId.Equals(id));

            //filter by status
            if (status.Length != 0)
                query = query.Where(news => status.Contains(news.Status));

            //filter by apartmentId
            if (!string.IsNullOrEmpty(apartmentId))
                query = query.Where(news => news.ApartmentId.Equals(apartmentId));

            //filter by date
            if (date != DateTime.MinValue)
                query = query.Where(news => news.ReleaseDate.Equals(date.Date));

            //add include
            if (include.Length > 0)
            {
                foreach(var item in include)
                {
                    if (item.Equals(nameof(News.Resident)))
                        query = query.Include(news => news.Resident);
                    if(item.Equals(nameof(News.Apartment)))
                        query = query.Include(news => news.Apartment);
                }
            }

            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");
            }

            //paging
            int perPage = limit.GetValueOrDefault(10);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<News>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }
    }
}
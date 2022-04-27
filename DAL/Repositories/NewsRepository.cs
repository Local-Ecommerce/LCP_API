using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;

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
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="search"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<News>> GetNews(
            string id, string apartmentId, string type,
            DateTime? date, string search, int?[] status,
            int? limit, int? queryPage,
            List<string> sort, string[] include)
        {
            IQueryable<News> query = _context.News.Where(news => news.NewsId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(news => news.NewsId.Equals(id));

            //filter by status
            if (status != null && status.Length != 0)
                query = query.Where(news => status.Contains(news.Status));

            //filter by apartmentId
            if (!string.IsNullOrEmpty(apartmentId))
                query = query.Where(news => news.ApartmentId.Equals(apartmentId));

            //filter by date
            if (date != null)
                query = query.Where(news => news.ReleaseDate.Equals(date.Value.Date));

            //filter by type
            if (!string.IsNullOrEmpty(type))
                query = query.Where(news => news.Type.Equals(type));

            //search contains
            if (!string.IsNullOrEmpty(search))
                query = query.Where(news => news.Title.ToLower().Contains(search.ToLower()) ||
                                            news.Text.Contains(search.ToLower()));

            //add include
            if (include != null && include.Length > 0)
            {
                foreach (var item in include)
                {
                    if (item.Equals(nameof(News.Resident)))
                        query = query.Include(news => news.Resident);
                    if (item.Equals(nameof(News.Apartment)))
                        query = query.Include(news => news.Apartment);
                }
            }

            //sort
            if (sort != null)
            {
                var result = query.OrderBy(sort.First());

                foreach (var property in sort.Skip(1))
                    result = result.ThenBy(property);

                query = result;
            }

            //paging
            int perPage = limit.GetValueOrDefault(Int32.MaxValue);
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
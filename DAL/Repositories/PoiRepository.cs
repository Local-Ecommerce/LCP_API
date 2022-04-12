using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Linq.Dynamic.Core;

namespace DAL.Repositories
{
    public class PoiRepository : Repository<Poi>, IPoiRepository
    {
        public PoiRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get Poi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="isPriority"></param>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="search"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<Poi>> GetPoi(
            string id, string apartmentId, bool? isPriority, string type,
            DateTime date, string search,
            int?[] status, int? limit, int? queryPage,
            bool isAsc, string propertyName, string[] include)
        {
            IQueryable<Poi> query = _context.Pois.Where(poi => poi.PoiId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(poi => poi.PoiId.Equals(id));

            //filter by status
            if (status != null && status.Length != 0)
                query = query.Where(poi => status.Contains(poi.Status));

            //filter by apartmentId
            if (!string.IsNullOrEmpty(apartmentId))
                query = query.Where(poi => poi.ApartmentId.Equals(apartmentId));
            query = query.Where(poi => status.Contains(poi.Status));

            //filter by type
            if (!string.IsNullOrEmpty(type))
                query = query.Where(poi => poi.Type.Equals(type));

            //filter by date
            if (date != DateTime.MinValue)
                query = query.Where(poi => poi.ReleaseDate.Equals(date.Date));

            //search contains
            if (!string.IsNullOrEmpty(search))
                query = query.Where(poi => poi.Title.ToLower().Contains(search.ToLower()) ||
                                            poi.Text.Contains(search.ToLower()));

            //add include
            if (include != null && include.Length > 0)
            {
                foreach (var item in include)
                {
                    if (item.Equals(nameof(Poi.Resident)))
                        query = query.Include(poi => poi.Resident);
                    if (item.Equals(nameof(Poi.Apartment)))
                        query = query.Include(poi => poi.Apartment);
                }
            }

            if (isPriority != null && isPriority == true)
                query.OrderBy("Priority");

            //sort
            if (!string.IsNullOrEmpty(propertyName))
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");

            //paging
            int perPage = limit.GetValueOrDefault(Int32.MaxValue);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<Poi>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }
    }
}
using DAL.Models;
using DAL.Repositories.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DAL.Constants;
using System;
using System.Linq.Dynamic.Core;

namespace DAL.Repositories
{
    public class ApartmentRepository : Repository<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get Apartment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<Apartment>> GetApartment(
            string id, int?[] status, 
            int? limit, int? queryPage, 
            bool isAsc, string propertyName, string include)
        {
            IQueryable<Apartment> query = _context.Apartments.Where(ap => ap.ApartmentId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(ap => ap.ApartmentId.Equals(id));
            
            //filter by status
            if(status.Length != 0)
                query = query.Where(ap => status.Contains(ap.Status));

            //add include
            if (!string.IsNullOrEmpty(include))
                if (include.Equals("marketmanager"))
                    query = query.Include(ap => ap.Residents.Where(res => res.Type.Equals(ResidentType.MARKET_MANAGER)));

            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");
            }

            //paging
            int perPage = limit.GetValueOrDefault(10);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<Apartment>
            {
                List = await query.Take(perPage).Skip((page - 1) * perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }
    }
}

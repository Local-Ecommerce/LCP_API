using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ResidentRepository : Repository<Resident>, IResidentRepository
    {
        public ResidentRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get Resident
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="accountId"></param>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public async Task<PagingModel<Resident>> GetResident(
            string id, string apartmentId, string phoneNumber, string accountId,
            int?[] status, string type, int? limit, int? queryPage,
            bool isAsc, string propertyName)
        {
            IQueryable<Resident> query = _context.Residents.Where(r => r.ResidentId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(r => r.ResidentId.Equals(id));

            //filter by apartmentId
            if (!string.IsNullOrEmpty(apartmentId))
                query = query.Where(r => r.ApartmentId.Equals(apartmentId));

            //filter by accountId
            if (!string.IsNullOrEmpty(accountId))
                query = query.Where(r => r.AccountId.Equals(accountId));

            if (!string.IsNullOrEmpty(phoneNumber))
                query = query.Where(r => r.PhoneNumber.Equals(phoneNumber));

            //filter by status
            if (status != null && status.Length != 0)
                query = query.Where(r => status.Contains(r.Status));

            //filter by type
            if (!string.IsNullOrEmpty(type))
                query = query.Where(r => r.Type.Equals(type));

            //sort
            if (!string.IsNullOrEmpty(propertyName))
                query = isAsc ? query.OrderBy(c => propertyName) : query.OrderByDescending(c => propertyName);

            //paging
            int perPage = limit.GetValueOrDefault(Int32.MaxValue);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<Resident>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }
    }
}
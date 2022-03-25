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
    public class MerchantStoreRepository : Repository<MerchantStore>, IMerchantStoreRepository
    {
        public MerchantStoreRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="residentId"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<MerchantStore>> GetMerchantStore(
            string id, string apartmentId, string residentId,
            int?[] status, int? limit,
            int? queryPage, bool isAsc,
            string propertyName, string[] include)
        {
            IQueryable<MerchantStore> query = _context.MerchantStores.Where(ms => ms.MerchantStoreId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(ms => ms.MerchantStoreId.Equals(id));

            //filter by status
            if (status.Length != 0)
                query = query.Where(ms => status.Contains(ms.Status));

            //filter by apartmentId
            if (!string.IsNullOrEmpty(apartmentId))
                query = query.Where(ms => ms.ApartmentId.Equals(apartmentId));

            //filter by residentId
            if (!string.IsNullOrEmpty(residentId))
                query = query.Where(ms => ms.ResidentId.Equals(residentId));

            //add include
            if (include.Length > 0)
            {
                foreach (string item in include)
                {
                    switch (item)
                    {
                        case nameof(MerchantStore.Resident):
                            query = query.Include(ms => ms.Resident);
                            break;
                        case nameof(MerchantStore.Apartment):
                            query = query.Include(ms => ms.Apartment);
                            break;
                        case "menu":
                            query = query.Include(ms => ms.Menus);
                            break;
                    };
                }
            }

            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");
            }

            //paging
            int perPage = limit.GetValueOrDefault(Int32.MaxValue);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<MerchantStore>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }


        /// <summary>
        /// Get Merchant Stores By Ids
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<List<MerchantStore>> GetMerchantStoresByIdsAndApartmentId(List<string> ids, string apartmentId)
        {
            return await _context.MerchantStores.Where(ms => ids.Contains(ms.MerchantStoreId) && ms.ApartmentId.Equals(apartmentId))
                                    .ToListAsync();
        }
    }
}
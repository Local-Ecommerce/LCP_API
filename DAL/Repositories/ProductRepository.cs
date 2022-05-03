using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DAL.Constants;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="apartmentId"></param>
        /// <param name="categoryId"></param>
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<PagingModel<Product>> GetProduct(
            string id = default, int?[] status = default, string apartmentId = default, string categoryId = default,
            string search = default, int? limit = default, int? queryPage = default,
            bool isAsc = default, string propertyName = default, string[] include = default, string residentId = default)
        {
            IQueryable<Product> query = _context.Products;

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(p => p.ProductId.Equals(id));

            //filter by status
            if (status != null && status.Length != 0)
                query = query.Where(p => status.Contains(p.Status));

            //filter by apartmentId
            if (!string.IsNullOrEmpty(apartmentId))
                query = query.Include(p => p.Resident).Where(p => p.Resident.ApartmentId.Equals(apartmentId));

            //filter by categoryId
            if (!string.IsNullOrEmpty(categoryId))
                query = query.Where(p => p.SystemCategoryId.Equals(categoryId));

            //filter by residentId
            if (!string.IsNullOrEmpty(residentId))
                query = query.Where(p => p.ResidentId.Equals(residentId));

            //filter by search
            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => (!string.IsNullOrEmpty(p.ProductName) && p.ProductName.ToLower().Contains(search.ToLower())) ||
                                           (!string.IsNullOrEmpty(p.BriefDescription) && p.BriefDescription.ToLower().Contains(search.ToLower())) ||
                                            (!string.IsNullOrEmpty(p.Description) && p.Description.ToLower().Contains(search.ToLower())) ||
                                            (!string.IsNullOrEmpty(p.ProductCode) && p.ProductCode.ToLower().Contains(search.ToLower())));

            //add include
            if (include != null && include.Length > 0)
            {
                foreach (var item in include)
                {
                    switch (item)
                    {
                        case "base":
                            query = query.Where(p => p.BelongTo != null).Include(p => p.BelongToNavigation);
                            break;
                        case "related":
                            query = query.Where(p => p.BelongTo == null)
                            .Include(p => p.InverseBelongToNavigation.Where(related => related.Status != (int)ProductStatus.DELETED_PRODUCT));
                            break;
                        case "menu":
                            query = query.Include(p => p.ProductInMenus)
                                        .ThenInclude(pim => pim.Menu);
                            break;
                        case "feedback":
                            query = query.Include(p => p.Feedbacks);
                            break;
                    }
                }
            }
            else
                query = query.Where(p => p.BelongTo == null);

            //sort
            if (!string.IsNullOrEmpty(propertyName))
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");

            //paging
            int perPage = limit.GetValueOrDefault(Int32.MaxValue);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<Product>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }


        /// <summary>
        /// Get Product Include Store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product> GetProductIncludeStore(string id)
        {
            return (await _context.Products.Where(p => p.ProductId.Equals(id))
                                            .Include(p => p.ProductInMenus)
                                            .ThenInclude(pim => pim.Menu)
                                            .ThenInclude(m => m.MerchantStore)
                                            .ToListAsync()).First();
        }


        /// <summary>
        /// Get Products By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Product>> GetProductsById(string id)
        {
            return await _context.Products.Where(p => p.ProductId.Equals(id) || (p.BelongTo != null && p.BelongTo.Equals(id))).ToListAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
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
        /// <param name="type"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<Product>> GetProduct(
            string id, int?[] status, string apartmentId, string type,
            int? limit, int? queryPage,
            bool isAsc, string propertyName, string include)
        {
            IQueryable<Product> query = _context.Products;

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(p => p.ProductId.Equals(id));

            //filter by status
            if (status.Length != 0)
                query = query.Where(p => status.Contains(p.Status));

            //filter by apartmentId
            if (apartmentId != null)
                query = query.Include(p => p.Resident).Where(p => p.Resident.ApartmentId.Equals(apartmentId));

            //filter by type
            if (type != null)
                if (type.Equals("all"))
                    query = query.Include(p => p.ProductCategories);
                else
                    query = query.Include(p => p.ProductCategories.Where(pc => pc.SystemCategory.Type.Contains(type)));

            //add include
            if (!string.IsNullOrEmpty(include))
            {
                if (include.Equals("related"))
                    query = query.Where(p => p.BelongTo == null).Include(p => p.InverseBelongToNavigation);
                else if (include.Equals("base"))
                    query = query.Where(p => p.BelongTo != null).Include(p => p.BelongToNavigation);
                else if (include.Equals("productCategory"))
                    query = query.Where(p => p.BelongTo == null).Include(p => p.ProductCategories);
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
    }
}